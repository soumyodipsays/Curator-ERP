// Models/ViewModels/NavViewModel.cs
namespace Curator.Web.Models.ViewModels
{
    public class NavViewModel
    {
        public int CartCount { get; set; } = 0;
        public string ActivePage { get; set; } = "Home";
        public bool IsLoggedIn { get; set; } = false;
        public string UserName { get; set; } = "";
    }
}