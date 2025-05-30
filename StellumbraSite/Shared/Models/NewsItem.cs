namespace StellumbraSite.Shared.Models
{
    public class NewsItem
    {
        public string Title { get; private set; }
        public string TitleImagePath { get; private set; }
        public string Date { get; private set; }
        public string Caption { get; private set; }
        public NewsContent[] Content { get; private set; }
        public NewsItem(string title, string titleImagePath, string date, string caption, NewsContent[] content)
        {
            Title = title;
            TitleImagePath = titleImagePath;
            Date = date;
            Caption = caption;
            Content = content;
        }
    }
}
