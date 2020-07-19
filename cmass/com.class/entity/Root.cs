using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODING.com
{
    public class Words_resultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string words { get; set; }
    }

    public class Word
    {
        /// <summary>
        /// 
        /// </summary>
        public string log_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int words_result_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Words_resultItem> words_result { get; set; }
    }
}
