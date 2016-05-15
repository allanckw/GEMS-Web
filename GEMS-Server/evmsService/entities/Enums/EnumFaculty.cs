using System.Runtime.Serialization;

namespace evmsService.entities
{
    [DataContract(Name = "Faculty")]
    public enum Faculties
    {
        //[EnumMember(Value="Faculty Of Science")]
        [EnumMember]
        Faculty_of_Science,
        //[EnumMember(Value = "Faculty of Engineering")]
        [EnumMember]
        Faculty_of_Engineering,
        //[EnumMember(Value = "Faculty Of Arts and Social Science")]
        [EnumMember]
        Faculty_of_Arts_and_Social_Science,
        //[EnumMember(Value = "Faculty of Dentistry")]
        [EnumMember]
        Faculty_of_Dentistry,
        //[EnumMember(Value = "Faculty Of Law")]
        [EnumMember]
        Faculty_of_Law,
        //[EnumMember(Value = "School of Computing")]
        [EnumMember]
        School_of_Computing,
        //[EnumMember(Value = "Yong Loo Lin School of Medicine")]
        [EnumMember]
        Yong_Loo_Lin_School_of_Medicine,
        //[EnumMember(Value = "Yong Siew Toh Conservatory Of Music")]
        [EnumMember]
        Yong_Siew_Toh_Conservatory_Of_Music,
        //[EnumMember(Value = "School of Design and Environment")]
        [EnumMember]
        School_of_Design_and_Environment,
        //[EnumMember(Value = "School of Business")]
        [EnumMember]
        School_of_Business,
        //[EnumMember(Value = "Centre for Development of Teaching and Learning")]
        [EnumMember]
        Centre_for_Development_of_Teaching_and_Learning,
        //[EnumMember(Value = "Centre for Instructional Technology")]
        [EnumMember]
        Centre_for_Instructional_Technology,
        //[EnumMember(Value = "Computing Commons")]
        [EnumMember]
        Computing_Commons,
        //[EnumMember(Value = "EduSports")]
        [EnumMember]
        EduSports,
        //[EnumMember(Value = " Khoo Teck Puat Advanced Surgery Training Centre")]
        [EnumMember]
        Khoo_Teck_Puat_Advanced_Surgery_Training_Centre,
        //[EnumMember(Value = "Lee Kuan Yew School of Public Policy")]
        [EnumMember]
        Lee_Kuan_Yew_School_of_Public_Policy,
        //[EnumMember(Value = "PC Cluster")]
        [EnumMember]
        PC_Cluster,
        //[EnumMember(Value = "Science Cluster")]
        [EnumMember]
        Science_Cluster,
        //[EnumMember(Value = "University Town")]
        [EnumMember]
        UTown
    }

    //Type: CC - Computer Cluster; CR - Conference Room; ER - Executive Room; 
    //LT - Lecture Theatre; SR - Seminar Room; TR - Teaching Room
    [DataContract(Name = "RoomTypes")]
    public enum RoomType
    {
        [EnumMember]

        Computer_Cluster = 0, //Computer Cluster,
        [EnumMember]
        Conference_Room = 1, //Conference Room,
        [EnumMember]
        Executive_Room = 2, //Executive Room
        [EnumMember]
        Lecture_Theatre = 3, //Lecture Theatre
        [EnumMember]
        Seminar_Room = 4, //Seminar Room
        [EnumMember]
        Teaching_Room = 5, //Teaching Room
        [EnumMember]
        Lab = 6, //Any Kind of  Lab
        [EnumMember]
        Outdoors = 7, //Any Kind of out door location

        [EnumMember]
        All = 99
    }
}