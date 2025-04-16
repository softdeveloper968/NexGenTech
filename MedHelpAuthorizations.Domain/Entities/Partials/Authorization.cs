using System;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Domain.Entities
{
    public partial class Authorization
    {
        //public bool AuthorizationNeedsDocuments(Authorization authorization)
        //{
            
        //    if(authorization.AuthType.Name.Contains("PRP"))
        //    {
        //        var recentReferralFound = false;

        //        ////If original PRP auth that is not succeded by another or is not the successor of another. 
        //        if (!authorization.InitialAuthorizations.Any() && !authorization.SucceededAuthorizations.Any())
        //        {
        //            //Age does not matter; only need a referral for any age. 
        //            return !authorization.Documents.Any(x => x?.DocumentType?.Name == "Referral");
        //        }

        //        //If the first concurrent; check to see if referral on intial is recent enough that we don't need another one (within 60 Days). 
        //        if (!authorization.InitialAuthorizations.Any() && authorization.SucceededAuthorizations.Any())
        //        {
        //            var succeededAuth = authorization.SucceededAuthorizations.Where(x => x.SucceededAuthorization.Id == authorization.Id).FirstOrDefault();
        //            var intialReferralDocument = succeededAuth.InitialAuthorization?.Documents?.Where(x => x.DocumentType.Name == "Referral").OrderByDescending(x => x.DocumentDate).FirstOrDefault();

        //            if(intialReferralDocument != null && intialReferralDocument.DocumentDate != null && authorization.StartDate != null && ((authorization.StartDate.Value - intialReferralDocument.DocumentDate.Value).TotalDays <= 60))
        //            {
        //                recentReferralFound = true;
        //            }
        //        }

        //        //If has been a concurrent auth. 
        //        if(authorization.SucceededAuthorizations.Any())
        //        {
        //            //If < 18 need 2 types required docs. Can use initial if recent
        //            if (authorization.Patient.Age < 18)
        //                return (!authorization.Documents.Any(x => x?.DocumentType?.Name == "Referral") 
        //                        || !authorization.Documents.Any(x => x?.DocumentType?.Name == "TreatmentPlan")) 
        //                    && !recentReferralFound;
        //            else
        //                //If >= 18 - need 3 types of required docs. Can use initial referral if 1st concurrent and recent. 
        //                return (!authorization.Documents.Any(x => x?.DocumentType?.Name == "Referral") 
        //                        || !authorization.Documents.Any(x => x?.DocumentType?.Name == "TreatmentPlan")
        //                        || !authorization.Documents.Any(x => x?.DocumentType?.Name == "DLA20")) 
        //                    && !recentReferralFound;
        //        }
        //    }
            
        //    return false;
        //}

        public IList<string> GetNeededDocumentTypes(Authorization authorization)
        {
            IList<string> neededDocumentTypes = new List<string>();

            if (authorization.AuthType.Name.Contains("PRP"))
            {
                var recentReferralFound = false;

                ////If original PRP auth that is not succeded by another or is not the successor of another. 
                if (!Enumerable.Any<ConcurrentAuthorization>(authorization.InitialAuthorizations) && !Enumerable.Any<ConcurrentAuthorization>(authorization.SucceededAuthorizations))
                {
                    //Age does not matter; only need a referral for any age. 
                    if(!Enumerable.Any<Document>(authorization.Documents, x => x?.DocumentType?.Name == "Referral"))
                    {
                        neededDocumentTypes.Add("Referral");

                        return neededDocumentTypes;
                    }
                }

                //If the first concurrent; check to see if referral on intial is recent enough that we don't need another one (within 60 Days). 
                if (!Enumerable.Any<ConcurrentAuthorization>(authorization.InitialAuthorizations) && Enumerable.Any<ConcurrentAuthorization>(authorization.SucceededAuthorizations))
                {
                    var succeededAuth = Enumerable.Where<ConcurrentAuthorization>(authorization.SucceededAuthorizations, x => x.SucceededAuthorization.Id == authorization.Id).FirstOrDefault();
                    var intialReferralDocument = Enumerable.OrderByDescending<Document, DateTime?>(succeededAuth.InitialAuthorization?.Documents?.Where(x => x.DocumentType.Name == "Referral"), x => x.DocumentDate).FirstOrDefault();

                    if (intialReferralDocument != null && intialReferralDocument.DocumentDate != null && authorization.StartDate != null && ((authorization.StartDate.Value - intialReferralDocument.DocumentDate.Value).TotalDays <= 60))
                    {
                        recentReferralFound = true;
                    }
                }

                //If has been a concurrent auth. 
                if (Enumerable.Any<ConcurrentAuthorization>(authorization.SucceededAuthorizations))
                {
                    //Can use initial referral if recent
                    if(!Enumerable.Any<Document>(authorization.Documents, x => x?.DocumentType?.Name == "Referral") && !recentReferralFound)
                        neededDocumentTypes.Add("Referral");

                    if(!Enumerable.Any<Document>(authorization.Documents, x => x?.DocumentType?.Name == "TreatmentPlan"))
                        neededDocumentTypes.Add("Treatment Plan");

                    //If >= 18 - need additional treatment plan document
                    if (authorization.Patient.Person.Age >= 18 && !Enumerable.Any<Document>(authorization.Documents, x => x?.DocumentType?.Name == "DLA20"))
                        neededDocumentTypes.Add("DLA20");
                }
            }

            return neededDocumentTypes;
        }

        //     **** Needs documents based on patient age - Notes
        //         PRP documents Age 0-17:

        //         Initial PRP Auh:  require a referral only

        //         Concurrent PRP Auth: requires a referral and treatment plan


        //         PRP documents Age 18-99:

        //         Initial PRP Auh:  require a referral only

        //         Concurrent PRP Auth: requires a referral, treatment plan and DLA


        //         Can have concurrent upon concurrent.Most scene yet is 3 but could be more.


        //         We require a treatment plan, DLA, new referral for all concurrents after 1st concurrent auth.


        //         once we obtain the initial, we get the next auth, the concurrent (30 days prior to the initial expiring and then; if the patient is still receiving serves, 
        //as the 1st concurrent comes to the last 30 days of this auth, we would request another concurrent with needing new documents). Let me know if you need anything else.

        //        You have to have at least a pending initial auth before we would enter a concurrent auth(if we have all required docs). We try not to even enter the concurrent until the initial is approved, due to issues this will cause (if the initial is denied and the concurrent is approved these dates would have to be updated in Incedo and changed to cover what we entered as the initial. Units would also change). 

        //All we need for an initial is the referral, we can’t obtain an auth without it. 

    }
}
