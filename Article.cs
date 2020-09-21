using System;
class Article
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string cover_image { get; set; }
    public string[] tags { get; set; }
    public string url { get; set; }
    public bool published { get; set; }
    public string created_at { get; set; }
    public string published_timestamp { get; set; }
    public int comments_count { get; set; }
    public int public_reactions_count { get; set; }
    public int page_views_count { get; set; }
    public string body_markdown { get; set; }

    public override string ToString()
    {
        return $"\nTitle: {title}\nDescription: {description}\nPublished: {published}\nPublish Time: {GetPublishedDate()}\nUrl: {url}\nTag List: {GetTagList()}\nPage Views: {page_views_count}\nReactions: {public_reactions_count}\nComments: {comments_count}\n";
    }

    public string GetTagList ()
    {
        string tagList = "";

        foreach (var tag in tags)
        {
            tagList += $"{tag}, ";
        }

        return tagList.Substring(0, tagList.Length-3);
    }

    public DateTime GetPublishedDate()
    {
        return DateTime.Parse(published_timestamp);
    }
}