using System;
using System.Collections.Generic;
using System.Text;

namespace owner.Model
{
    public enum MenuItemType
    {
        MenuItem1,
        MenuItem2,
        MenuItem3,
        Logout
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
