namespace Application.Email.DataTransferObjects
{
    public class EmailAttachmentDto
    {
        public EmailAttachmentDto(string fileName, byte[] content, string mediaType)
        {
            FileName = fileName;
            Content = content;
            MediaType = mediaType;
        }

        public string FileName { get; }

        public byte[] Content { get; }

        public string MediaType { get; }
    }
}
