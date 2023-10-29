using Microsoft.AspNetCore.Mvc;

namespace Clothings_Store.Areas.Identity.Pages.Shared.Components.MessagePage
{
    [ViewComponent]
    public class MessagePage : ViewComponent
    {
        public const string COMPONENTNAME = "MessagePage";
        // Dữ liệu nội dung trang thông báo
        public class Message
        {
            public string title { set; get; } = "Thông báo";    
            public string htmlcontent { set; get; } = "";       
            public string urlredirect { set; get; } = "/";      
            public int secondwait { set; get; } = 3;          
        }
        public MessagePage() { }
        public IViewComponentResult Invoke(Message message)
        {
            // Thiết lập Header của HTTP Respone - chuyển hướng về trang đích
            this.HttpContext.Response.Headers.Add("REFRESH", $"{message.secondwait};URL={message.urlredirect}");
            return View(message);
        }
    }
}
