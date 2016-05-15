using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using evmsService.entities;

//Need a add a reference lib
using System.ServiceModel.Web;

//http://dotnetninja.wordpress.com/2008/05/02/rest-service-with-wcf-and-json/
//http://www.knowledgebaseworld.blogspot.sg/2010/06/calling-wcf-service-from-iphone.html
//http://www.knowledgebaseworld.blogspot.sg/2010/06/wcf-service-with-webhttpbinding.html
//http://msdn.microsoft.com/en-us/library/dd203052.aspx
[ServiceContract]
interface IMobile
{
    //http://zdravevski.com/blog/2012/05/27/json-serialized-date-passed-between-ios-and-wcf-and-vice-versa/

    // http://localhost:62709/EvmsService.svc/REST/GetAllEvents?fd=2005-11-13%205%3A30%3A00&td=2016-11-13%205%3A30%3A00

    [OperationContract]
    [WebGet(UriTemplate = "GetAllEventsWithTag?fd={fromDate}&td={toDate}&tag={tag}",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<M_ShortEvent> m_GetEventsWithTag(DateTime fromDate, DateTime toDate, string tag);

    [OperationContract]
    [WebGet(UriTemplate = "GetAllEvents?fd={fromDate}&td={toDate}",
    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<M_ShortEvent> m_GetEvents(DateTime fromDate, DateTime toDate);

    [OperationContract]
    [WebGet(UriTemplate = "GetEvent?eid={eventID}",
    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    M_Event m_GetEventObj(int eventID);

    [OperationContract]
    [WebGet(UriTemplate = "GetEventProgs?eid={eventID}",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<M_ShortProgram> m_GetEventProgramme(int eventID);

    [OperationContract]
    [WebGet(UriTemplate = "GetProg?pid={progID}",
    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    M_Program m_GetProgramme(int progID);

    [OperationContract]
    [WebGet(UriTemplate = "GetEventGuests?eid={eventID}",
       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<M_ShortGuest> m_GetEventGuests(int eventID);

    [OperationContract]
    [WebGet(UriTemplate = "GetGuest?gid={guestID}",
    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    M_Guest m_GetGuest(int guestID);

    [OperationContract]
    [WebGet(UriTemplate = "Auth?uid={uID}&pwd={pwd}",
    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    string m_Authenticate(string uid, string pwd);

    [OperationContract]
    [WebGet(UriTemplate = "CanViewTask?key={key}&eventID={evID}",
    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    bool m_CanViewTask(string key, int evID);

    [OperationContract]
    [WebGet(UriTemplate = "ViewMyTask?key={key}&eventID={evID}",
    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<M_ShortTask> m_ViewMyTask(string key, int evID);

    [OperationContract]
    [WebGet(UriTemplate = "ViewEventTask?key={key}&eventID={evID}",
    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    List<M_ShortTask> m_ViewEventTask(string key, int evID);

    [OperationContract]
    [WebGet(UriTemplate = "GetTask?key={key}&taskID={taskID}",
    RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
    M_Task m_GetTask(string key, int taskID);
}
