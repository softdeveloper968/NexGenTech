using System.ComponentModel;

namespace MedHelpAuthorizations.Application.Enums
{
    public enum UploadType
    {
        [Description(@"Images\Products")]
        Product,

        [Description(@"Images\ProfilePictures")]
        ProfilePicture,

        [Description(@"Documents")]
        Document,

        [Description(@"Images\Patients")]
        Patient,

        [Description(@"Images\Authorizations")]
        Authorization,

        [Description(@"IntegratedServices\InputDocuments")]
        InputDocument,
    }
}