using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.ServiceModel;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class ReviewController
    {
        public static void Review(User user, int EventID, int serviceID, int rating, DateTime reviewDate, string reviewDescription)
        {
            bool allow = false;
            
            if ((user.isEventOrganizer) || (user.isSystemAdmin))
                allow = true;
            
            if (!allow)
            {
                if (!user.isAuthorized(EventController.GetEvent(EventID), EnumFunctions.Manage_Items))
                    throw new FaultException<SException>(new SException(),
                       new FaultReason("Invalid User, User Does Not Have Rights To Add New Review!"));
            }
            try
            {
                DAL dalDataContext = new DAL();
                Table<Review> reviews = dalDataContext.reviews;

                Review existingReview = (from review in dalDataContext.reviews
                                         where review.UserID == user.UserID &&
                                         review.ServiceID == serviceID
                                         select review).FirstOrDefault();

                if (existingReview == null)
                {
                    Review creatingReview = new Review(serviceID, user.UserID, user.Name, rating,
                        reviewDate, reviewDescription);

                    reviews.InsertOnSubmit(creatingReview);
                }
                else
                {
                    existingReview.UserID = user.UserID;
                    existingReview.UserName = user.Name;
                    existingReview.ServiceID = serviceID;
                    existingReview.Rating = rating;
                    existingReview.ReviewDate = reviewDate;
                    existingReview.ReviewDescription = reviewDescription;
                }
                reviews.Context.SubmitChanges();

                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Adding New Review, Please Try Again!"));
            }
        }

        public static Review GetReview(string UserID, int ServiceID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                Review existingReview = (from review in dalDataContext.reviews
                                         where review.UserID == UserID &&
                                         review.ServiceID == ServiceID
                                         select review).FirstOrDefault();
                return existingReview;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                       new FaultReason("An Error occured While Retrieving Review Data, Please Try Again!"));
            }
        }

        public static void DeleteReview(User user, string UserID, int ServiceID)
        {

            Review r = GetReview(UserID, ServiceID);

            if (!user.isSystemAdmin && !(user.UserID == UserID))
                throw new FaultException<SException>(new SException(),
                   new FaultReason("Invalid User, User Does Not Have Rights To Delete Review!"));
            //chk if user can do this anot

            DAL dalDataContext = new DAL();

            try
            {
                r = (from review in dalDataContext.reviews
                                 where review.UserID == UserID &&
                                 review.ServiceID == ServiceID
                                 select review).FirstOrDefault();

                

                dalDataContext.reviews.DeleteOnSubmit(r);
                dalDataContext.SubmitChanges();
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured While Deleting Review, Please Try Again!"));
            }
        }

        public static List<Review> ViewReview(int ServiceID)
        {
            try
            {
                DAL dalDataContext = new DAL();

                List<Review> reviews = (from review in dalDataContext.reviews
                                        where review.ServiceID == ServiceID
                                        select review).ToList<Review>();

                return reviews;
            }
            catch
            {
                throw new FaultException<SException>(new SException(),
                   new FaultReason("An Error occured Retrieving Review Data, Please Try Again!"));
            }
        }
    }
}