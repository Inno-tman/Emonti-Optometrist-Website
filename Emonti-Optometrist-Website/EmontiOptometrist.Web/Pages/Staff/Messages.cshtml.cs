using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages.Staff;

public class MessagesModel : PageModel
{
    private readonly MessageDatabase _msgDb;

    public MessagesModel(MessageDatabase msgDb)
    {
        _msgDb = msgDb;
    }

    public List<ConversationSummary> Conversations { get; set; } = new();
    public List<MessageDto> ThreadMessages { get; set; } = new();
    public int? SelectedConversationId { get; set; }
    public string? SelectedSubject { get; set; }
    public string? SelectedCustomerName { get; set; }
    public bool HasConversations => Conversations.Count > 0;
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public IActionResult OnGet(int? conv)
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return RedirectToPage("/Login");

        Conversations = _msgDb.GetConversationsForStaff();

        if (conv.HasValue)
        {
            SelectedConversationId = conv.Value;
            ThreadMessages = _msgDb.GetConversationMessages(conv.Value);
            SelectedSubject = _msgDb.GetConversationSubject(conv.Value);
            var c = Conversations.FirstOrDefault(x => x.ConversationId == conv.Value);
            SelectedCustomerName = c?.CustomerName ?? "";
            _msgDb.MarkConversationRead(conv.Value, "Staff");
        }

        SuccessMessage = TempData["SuccessMessage"]?.ToString();
        ErrorMessage = TempData["ErrorMessage"]?.ToString();

        return Page();
    }

    public IActionResult OnPostReply(int conversationId, string body)
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return RedirectToPage("/Login");

        var staffId = HttpContext.Session.GetString("Staff_ID") ?? "";
        if (string.IsNullOrWhiteSpace(body))
        {
            TempData["ErrorMessage"] = "Please enter a message.";
            return RedirectToPage(new { conv = conversationId });
        }

        _msgDb.ReplyToConversation(conversationId, staffId, "Staff", body.Trim(), staffId);
        TempData["SuccessMessage"] = "Reply sent successfully.";
        return RedirectToPage(new { conv = conversationId });
    }
}
