using System.Collections.Generic;

namespace BASE.Model.SecurityMatrix
{
    public class ActionViewModel
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
    }

    public class ScreenViewModel
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
        public List<ActionViewModel> Actions { get; set; }
    }
}
