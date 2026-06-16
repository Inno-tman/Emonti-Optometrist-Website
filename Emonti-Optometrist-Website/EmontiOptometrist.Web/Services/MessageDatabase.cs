using Microsoft.Data.Sqlite;

namespace EmontiOptometrist.Web.Services;

public class MessageDatabase
{
    private readonly string _connectionString;

    public MessageDatabase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    public int CreateConversation(string custId, string subject, string body)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        using var maxCmd = conn.CreateCommand();
        maxCmd.CommandText = "SELECT COALESCE(MAX(Conversation_ID), 0) + 1 FROM Messages";
        int convId = Convert.ToInt32(maxCmd.ExecuteScalar());

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Messages (Conversation_ID, Cust_ID, Sender_Role, Subject, Message_Body, Created_At)
            VALUES (@ConvId, @CustId, 'Customer', @Subject, @Body, datetime('now', 'localtime'))";
        cmd.Parameters.AddWithValue("@ConvId", convId);
        cmd.Parameters.AddWithValue("@CustId", custId);
        cmd.Parameters.AddWithValue("@Subject", subject);
        cmd.Parameters.AddWithValue("@Body", body);
        cmd.ExecuteNonQuery();

        return convId;
    }

    public void ReplyToConversation(int conversationId, string senderId, string senderRole, string body, string? staffId = null)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        using var getCustCmd = conn.CreateCommand();
        getCustCmd.CommandText = "SELECT DISTINCT Cust_ID FROM Messages WHERE Conversation_ID = @ConvId AND Cust_ID IS NOT NULL";
        getCustCmd.Parameters.AddWithValue("@ConvId", conversationId);
        var custId = getCustCmd.ExecuteScalar()?.ToString() ?? "";

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Messages (Conversation_ID, Cust_ID, Staff_ID, Sender_Role, Message_Body, Created_At)
            VALUES (@ConvId, @CustId, @StaffId, @SenderRole, @Body, datetime('now', 'localtime'))";
        cmd.Parameters.AddWithValue("@ConvId", conversationId);
        cmd.Parameters.AddWithValue("@CustId", custId);
        cmd.Parameters.AddWithValue("@StaffId", staffId ?? (senderRole == "Staff" ? senderId : null));
        cmd.Parameters.AddWithValue("@SenderRole", senderRole);
        cmd.Parameters.AddWithValue("@Body", body);
        cmd.ExecuteNonQuery();
    }

    public List<ConversationSummary> GetConversationsForStaff()
    {
        var result = new List<ConversationSummary>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT 
                m.Conversation_ID,
                m.Subject,
                m.Cust_ID,
                c.Customer_Name,
                c.Customer_Surname,
                (SELECT Message_Body FROM Messages WHERE Conversation_ID = m.Conversation_ID ORDER BY Created_At DESC LIMIT 1) AS LastMessage,
                (SELECT Created_At FROM Messages WHERE Conversation_ID = m.Conversation_ID ORDER BY Created_At DESC LIMIT 1) AS LastDate,
                (SELECT COUNT(*) FROM Messages WHERE Conversation_ID = m.Conversation_ID AND Sender_Role = 'Customer' AND Is_Read = 0) AS UnreadCount
            FROM Messages m
            LEFT JOIN customer c ON m.Cust_ID = c.Cust_ID
            WHERE m.Sender_Role = 'Customer'
            GROUP BY m.Conversation_ID
            ORDER BY LastDate DESC";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new ConversationSummary
            {
                ConversationId = Convert.ToInt32(reader["Conversation_ID"]),
                Subject = reader["Subject"]?.ToString() ?? "",
                CustId = reader["Cust_ID"]?.ToString() ?? "",
                CustomerName = $"{reader["Customer_Name"]} {reader["Customer_Surname"]}",
                LastMessage = reader["LastMessage"]?.ToString() ?? "",
                LastDate = reader["LastDate"]?.ToString() ?? "",
                UnreadCount = Convert.ToInt32(reader["UnreadCount"])
            });
        }
        return result;
    }

    public List<ConversationSummary> GetConversationsForCustomer(string custId)
    {
        var result = new List<ConversationSummary>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT 
                Conversation_ID,
                Subject,
                (SELECT Message_Body FROM Messages WHERE Conversation_ID = m.Conversation_ID ORDER BY Created_At DESC LIMIT 1) AS LastMessage,
                (SELECT Created_At FROM Messages WHERE Conversation_ID = m.Conversation_ID ORDER BY Created_At DESC LIMIT 1) AS LastDate,
                (SELECT COUNT(*) FROM Messages WHERE Conversation_ID = m.Conversation_ID AND Sender_Role = 'Staff' AND Is_Read = 0) AS UnreadCount
            FROM Messages m
            WHERE Cust_ID = @CustId
            GROUP BY Conversation_ID
            ORDER BY LastDate DESC";
        cmd.Parameters.AddWithValue("@CustId", custId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new ConversationSummary
            {
                ConversationId = Convert.ToInt32(reader["Conversation_ID"]),
                Subject = reader["Subject"]?.ToString() ?? "",
                LastMessage = reader["LastMessage"]?.ToString() ?? "",
                LastDate = reader["LastDate"]?.ToString() ?? "",
                UnreadCount = Convert.ToInt32(reader["UnreadCount"])
            });
        }
        return result;
    }

    public List<MessageDto> GetConversationMessages(int conversationId)
    {
        var result = new List<MessageDto>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT * FROM Messages
            WHERE Conversation_ID = @ConvId
            ORDER BY Created_At ASC";
        cmd.Parameters.AddWithValue("@ConvId", conversationId);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            result.Add(new MessageDto
            {
                MessageId = Convert.ToInt32(reader["Message_ID"]),
                ConversationId = Convert.ToInt32(reader["Conversation_ID"]),
                CustId = reader["Cust_ID"]?.ToString() ?? "",
                StaffId = reader["Staff_ID"]?.ToString() ?? "",
                SenderRole = reader["Sender_Role"]?.ToString() ?? "",
                Subject = reader["Subject"]?.ToString() ?? "",
                MessageBody = reader["Message_Body"]?.ToString() ?? "",
                CreatedAt = reader["Created_At"]?.ToString() ?? "",
                IsRead = Convert.ToInt32(reader["Is_Read"])
            });
        }
        return result;
    }

    public string? GetConversationSubject(int conversationId)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Subject FROM Messages WHERE Conversation_ID = @ConvId ORDER BY Created_At ASC LIMIT 1";
        cmd.Parameters.AddWithValue("@ConvId", conversationId);
        return cmd.ExecuteScalar()?.ToString();
    }

    public void MarkConversationRead(int conversationId, string byRole)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        if (byRole == "Staff")
            cmd.CommandText = "UPDATE Messages SET Is_Read = 1 WHERE Conversation_ID = @ConvId AND Sender_Role = 'Customer'";
        else
            cmd.CommandText = "UPDATE Messages SET Is_Read = 1 WHERE Conversation_ID = @ConvId AND Sender_Role = 'Staff'";
        cmd.Parameters.AddWithValue("@ConvId", conversationId);
        cmd.ExecuteNonQuery();
    }

    public int GetUnreadCountForStaff()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Messages WHERE Sender_Role = 'Customer' AND Is_Read = 0";
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public int GetUnreadCountForCustomer(string custId)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Messages WHERE Cust_ID = @CustId AND Sender_Role = 'Staff' AND Is_Read = 0";
        cmd.Parameters.AddWithValue("@CustId", custId);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}

public class ConversationSummary
{
    public int ConversationId { get; set; }
    public string Subject { get; set; } = "";
    public string CustId { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string LastMessage { get; set; } = "";
    public string LastDate { get; set; } = "";
    public int UnreadCount { get; set; }
}

public class MessageDto
{
    public int MessageId { get; set; }
    public int ConversationId { get; set; }
    public string CustId { get; set; } = "";
    public string StaffId { get; set; } = "";
    public string SenderRole { get; set; } = "";
    public string Subject { get; set; } = "";
    public string MessageBody { get; set; } = "";
    public string CreatedAt { get; set; } = "";
    public int IsRead { get; set; }
}
