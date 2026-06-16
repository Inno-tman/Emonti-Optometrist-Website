using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

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
    public bool HasConversations => Conversations.Count > 0;
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public IActionResult OnGet(int? conv)
    {
        if (!AuthSession.IsCustomerLoggedIn(HttpContext))
            return RedirectToPage("/Login");

        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId))
            return RedirectToPage("/Login");

        Conversations = _msgDb.GetConversationsForCustomer(custId);

        if (conv.HasValue)
        {
            SelectedConversationId = conv.Value;
            ThreadMessages = _msgDb.GetConversationMessages(conv.Value);
            SelectedSubject = _msgDb.GetConversationSubject(conv.Value);
            _msgDb.MarkConversationRead(conv.Value, "Customer");
        }

        SuccessMessage = TempData["SuccessMessage"]?.ToString();
        ErrorMessage = TempData["ErrorMessage"]?.ToString();

        return Page();
    }

    public IActionResult OnPostNew(string subject, string body)
    {
        if (!AuthSession.IsCustomerLoggedIn(HttpContext))
            return RedirectToPage("/Login");

        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
        {
            TempData["ErrorMessage"] = "Please fill in all fields.";
            return RedirectToPage();
        }

        var convId = _msgDb.CreateConversation(custId, subject.Trim(), body.Trim());
        TempData["SuccessMessage"] = "Message sent successfully.";
        return RedirectToPage(new { conv = convId });
    }

    public IActionResult OnPostReply(int conversationId, string body)
    {
        if (!AuthSession.IsCustomerLoggedIn(HttpContext))
            return RedirectToPage("/Login");

        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId) || string.IsNullOrWhiteSpace(body))
        {
            TempData["ErrorMessage"] = "Please enter a message.";
            return RedirectToPage(new { conv = conversationId });
        }

        _msgDb.ReplyToConversation(conversationId, custId, "Customer", body.Trim());
        TempData["SuccessMessage"] = "Reply sent successfully.";
        return RedirectToPage(new { conv = conversationId });
    }
}
