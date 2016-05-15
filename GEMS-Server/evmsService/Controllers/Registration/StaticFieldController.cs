using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class StaticFieldController
    {
        //public static void AddStaticField(User user, string FieldName, string FieldLabel)
        //{
        //    if (!u.isSystemAdmin)
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("Invalid User, User Does Not Have Rights To Add Static Field!"));
        //    try
        //    {
        //        DAL dalDataContext = new DAL();
        //        Table<StaticField> staticField = dalDataContext.staticFields;
        //        StaticField creatingStaticField = new StaticField(FieldName, FieldLabel);

        //        staticField.InsertOnSubmit(creatingStaticField);
        //        staticField.Context.SubmitChanges();


        //    }
        //    catch
        //    {
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("An Error occured While Adding New Static Field, Please Try Again!"));
        //    }
        //}
        //public static StaticField GetStaticField(int StaticFieldID)
        //{
        //    try
        //    {
        //        DAL dalDataContext = new DAL();

        //        StaticField existingStaticField = (from staticField in dalDataContext.staticFields
        //                                           where staticField.StaticFieldID == StaticFieldID
        //                                           select staticField).FirstOrDefault();

        //        if (existingStaticField == null)
        //        {
        //            throw new FaultException<SException>(new SException(),
        //               new FaultReason("Invalid Static FIeld"));
        //        }
        //        return existingStaticField;
        //    }
        //    catch
        //    {
        //        throw new FaultException<SException>(new SException(),
        //               new FaultReason("An Error occured While Retrieving Static FIeld Data, Please Try Again!"));
        //    }
        //}

        //public static void DeleteStaticField(User user, int StaticFieldID)
        //{
        //    //chk if user can do this anot

        //    if (!u.isSystemAdmin)
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("Invalid User, User Does Not Have Rights To Delete Static Field!"));

        //    DAL dalDataContext = new DAL();

        //    try
        //    {
        //        StaticField staticField = GetStaticField(StaticFieldID);

        //        dalDataContext.staticFields.DeleteOnSubmit(staticField);
        //        dalDataContext.SubmitChanges();
        //    }
        //    catch
        //    {
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("An Error occured While Deleting Static Field, Please Try Again!"));
        //    }
        //}
        //public static void EditStaticField(User user, int StaticFieldID, string FieldName, string FieldLabel)
        //{

        //    if (!u.isSystemAdmin)
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("Invalid User, User Does Not Have Rights To Edit this Static Fields!"));
        //    try
        //    {
        //        DAL dalDataContext = new DAL();

        //        StaticField sf = GetStaticField(StaticFieldID);

        //        if (sf == null)
        //        {

        //            throw new FaultException<SException>(new SException(),
        //               new FaultReason("Invalid Static Fields"));
        //        }
        //        else
        //        {
        //            sf.FieldName = FieldName;
        //            sf.FieldLabel = FieldLabel;
        //            dalDataContext.SubmitChanges();

        //        }
        //    }
        //    catch
        //    {
        //        throw new FaultException<SException>(new SException(),
        //           new FaultReason("An Error occured While Editing Static Field, Please Try Again!"));
        //    }
        //}
        public static List<StaticField> ViewStaticField()
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<StaticField> sf = (from staticField in dalDataContext.staticFields
                                        select staticField).ToList<StaticField>();

                return sf;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Static Field Data, Please Try Again!"));
            }
        }
    }
}