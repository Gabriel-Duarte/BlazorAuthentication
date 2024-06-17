using Radzen;

namespace BlazorAuthentication.Client.Model
{
    public class ThemeState
    {
        public string CurrentTheme { get; set; } = "humanistic";
        public static int[] pager = new int[] { 6, 7, 15, 20, 50, 1000 };
        public static int DefaulPageSize = 6;
        public static Density GridDensity = Density.Default;
    }

    public static class ModalState
    {
        public static string pSmall = "30%";
        public static string xSmall = "33%";
        public static string Small = "45%";
        public static string Medium = "66%";
        public static string Large = "98%";
        public static bool CloseDialogOnEsc = true;
        public static bool Draggable = false;
        public static bool Resizable = false;
        public static bool ShowClose = true;
    }
}
