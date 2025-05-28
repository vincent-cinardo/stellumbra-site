namespace StellumbraSite.Client.StellumbraSite.Shared.Models
{
    public class NewsItem
    {
        public string Title { get; private set; }
        public string Date { get; private set; }
        public NewsContent[] Content { get; private set; }
        public NewsItem(string title, string date, NewsContent[] content)
        {
            Title = title;
            Date = date;
            Content = content;
        }
    }
}
