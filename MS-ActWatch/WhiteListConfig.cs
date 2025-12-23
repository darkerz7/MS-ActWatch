namespace MS_ActWatch
{
    internal class WhiteListConfig
    {
        public List<string> Buttons { get; set; }
        public List<string> Triggers { get; set; }

        public WhiteListConfig()
        {
            Buttons = [];
            Triggers = [];
        }
    }
}
