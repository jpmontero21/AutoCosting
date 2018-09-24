using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCosting.Models
{
    public class IndividualButtonPartial
    {
        public string ButtonType { get; set; }
        public string Action { get; set; }
        public string Glyph { get; set; }
        public string Text { get; set; }

        public int? Id { get; set; }
        public string ActionParameters
        {
            get
            {
                StringBuilder param = new StringBuilder(@"/");
                if (this.Id != 0 && this.Id != null)
                {
                    param.Append(string.Format("{0}", Id));
                }
                return param.ToString().Substring(0, param.Length);
            }
        }
    }
}
