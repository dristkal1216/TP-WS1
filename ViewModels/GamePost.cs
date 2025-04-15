namespace TP_WS1.ViewModels
{
    internal class GamePost
    {
        public string GameName { get; set; }
        public int nbPost { get; set; }
        public object nbVue { get; set; }
        public string author { get; set; }
        public string lastUserActivity { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}