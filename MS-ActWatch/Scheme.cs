namespace MS_ActWatch
{
    internal class Scheme
    {
        public string Color_name { get; set; }
        public string Color_steamid { get; set; }
        public string Color_warning { get; set; }
        public string Color_enabled { get; set; }
        public string Color_disabled { get; set; }
        public string Color_use_trigger { get; set; }
        public string Color_use_button { get; set; }
        public string Color_entity_name { get; set; }
        public string Color_entity_id { get; set; }

        public string Server_name { get; set; }

        public Scheme()
        {
            Color_name = "{default}";
            Color_steamid = "{grey}";
            Color_warning = "{orange}";
            Color_enabled = "{green}";
            Color_disabled = "{red}";
            Color_use_trigger = "{lightblue}";
            Color_use_button = "{lightblue}";
            Color_entity_name = "{gold}";
            Color_entity_id = "{lime}";

            Server_name = "Server";
        }
    }
}
