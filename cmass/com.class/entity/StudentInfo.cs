using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODING.com
{
    class StudentInfo : IComparable<StudentInfo>
    {
 /*       public StudentInfo(string Name, string ClassNo, string Grade)
        {
            this.ClassNo = ClassNo;
            this.Name = Name;
            this.Grade = Grade;
        }
        */
        public string Uuid { get; set; }
        public string Name { get; set ; }
        public string ClassNo { get ; set ; }
        public int Grade { get ; set ; }
        public string AGrade { get; set; }

        //重写的CompareTo方法，根据Id排序
        public int CompareTo(StudentInfo other)
        {
            if (null == other)
            {
                return 1;//空值比较大，返回1
            }
            //return this.Id.CompareTo(other.Id);//升序
            return other.Grade.CompareTo(this.Grade);//降序
        }

        //重写ToString
        public override string ToString()
        {
            return "Uuid:"+Uuid+"  Class:" + ClassNo + "   Name:" + Name + "  Grade:"+Grade + "  AGrade:" + AGrade;
        }
    }
}
