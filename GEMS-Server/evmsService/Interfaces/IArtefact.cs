using System;
using System.Collections.Generic;
using System.ServiceModel;
using evmsService.Controllers;
using evmsService.entities;

[ServiceContract]
interface IArtefact
{
    //Folder Methods
    [OperationContract]
    [FaultContract(typeof(SException))]
    void CreateFolder(User user, int eventID, string folderName, string folderDesc, string remarks);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteFolder(User user, int eventID, string folderName);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void UpdateFolder(User user, int eventID, string folderName, string folderDesc, string remarks);

    [OperationContract]
    [FaultContract(typeof(SException))]
    List<WorkspaceFolders> GetWorkSpaceFolders(User user, int eventID);

    //File Methods
    [OperationContract]
    [FaultContract(typeof(SException))]
    void UploadFile(User user, int eventID, string folderName, string fileName, string fileDesc, string url);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void DeleteFile(User user, int eventID, string FolderName, string fileName);

    [OperationContract]
    [FaultContract(typeof(SException))]
    void UpdateFile(User user, int eventID, string folderName, string fileName, string fileDesc, string url);

    [OperationContract]
    [FaultContract(typeof(SException))]
    WorkspaceFiles GetWorkSpaceFile(User user, int eventID, string folderName, string fileName);

    //Load All
    [OperationContract]
    [FaultContract(typeof(SException))]
    List<WorkspaceFiles> GetWorkSpaceFiles(User user, int eventID, string folderName);

    [OperationContract]
    [FaultContract(typeof(SException))]
    WorkspaceFolders GetWorkSpaceFolder(User user, int eventID, string folderName);
    

    

    

    

}

