using System;
using System.Collections.Generic;
using System.Text;

namespace owner.WebService
{
    public class ResponseMsg
    {
        public string resp { get; set; }

        public string owner_profile { get; set; }

        public string owner_id { get; set; }
        public int program_fee { get; set; }
        public float income_rate { get; set; }

        public string message { get; set; }

        public string thread_comment_id { get; set; }
        public string zero_id { get; set; }
        public string estate_id { get; set; }

        public int comment_likes { get; set; }

        public string owner_video { get; set; }
        public string owner_zero_video { get; set; }

        public agreement_data[] agreement_data { get; set; }
    }

    public class agreement_data
    {
        public string agreement_id;
        public string agreement_image;
        public string agreement_name;
    }
}
