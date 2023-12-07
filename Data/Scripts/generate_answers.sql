declare @max int = 200;
 
with Statuses(StatusId) as
(
    SELECT 1   UNION ALL
    SELECT 2 
)
, Texts(Text) as
(
    SELECT N'The apocalypse began in a cubicle.'   UNION ALL
    SELECT N'Its walls were gray, its desk was gray, its floor was that kind of grayish tile that is designed to look dirty so nobody notices that it is actually dirty. Upon the floor was a chair and upon the chair was me. My name is Aaron Smith-Teller and I am twenty-two years old. I was fiddling with a rubber band and counting the minutes until my next break and seeking the hidden transcendent Names of God.'    UNION ALL
    SELECT N'“AR-ASH-KON-CHEL-NA-VAN-TSIR,” I chanted.

That wasn’t a hidden transcendent Name of God. That wasn’t surprising. During my six months at Countenance I must have spoken five hundred thousand of these words. Each had taken about five seconds, earned me about two cents, and cost a small portion of my dignity. None of them had been hidden transcendent Names of God.'   UNION ALL
    SELECT N'“AR-ASH-KON-CHEL-NA-VAN-TSIS,” ordered my computer, and I complied. “AR-ASH-KON-CHEL-NA-VAN-TSIS,” I said.

The little countdown clock on my desk said I had seven minutes, thirty nine seconds until my next break. That made a total of 459 seconds, which was appropriate, given that the numerical equivalents of the letters in the Hebrew phrase “arei miklat” meaning “city of refuge” summed to 459. There were six cities of refuge in Biblical Israel, three on either side of the Jordan River. There were six ten minute breaks during my workday, three on either side of lunch. None of this was a coincidence because nothing was ever a coincidence.'   UNION ALL
    SELECT N'“AR-ASH-KON-CHEL-NA-VAN-TSIT” was my computer’s next suggestion. “AR-ASH-KON-CHEL-NA-VAN-TSIT,” I said.

God created Man in His own image but He created everything else in His own image too. By learning the structure of one entity, like Biblical Israel, we learn facts that carry over to other structures, like the moral law, or the purpose of the universe, or my workday. This is the kabbalah. The rest is just commentary. Very, very difficult commentary, written in Martian, waiting to devour the unwary.'     UNION ALL
    SELECT N'“VIS-LAIGA-RON-TEPHENOR-AST-AST-TELISA-ROK-SUPH-VOD-APANOR-HOV-KEREG-RAI-UM”. My computer shifted to a different part of namespace, and I followed.

Thirty-six letters. A little on the long side. In general, the longer a Name, the harder to discover but the more powerful its effects. The longest known was the Wrathful Name, fifty letters. When spoken it levelled cities. The Sepher Raziel predicted that the Shem haMephorash, the Explicit Name which would capture God’s full essence and bestow near-omnipotence upon the speaker, would be seventy-two letters.'   UNION ALL
    SELECT N'“VIS-LAIGA-RON-TEPHENOR-AST-AST-TELISA-ROK-SUPH-VOD-APANOR-HOV-KEREG-RAI-US.”

People discovered the first few Names of God through deep understanding of Torah, through silent prayer and meditation, or even through direct revelation from angels. But American capitalism took one look at prophetic inspiration and decided it lacked a certain ability to be forced upon an army of low-paid interchangeable drones. Thus the modern method: hire people at minimum wage to chant all the words that might be Names of God, and see whether one of them starts glowing with holy light or summoning an angelic host to do their bidding. If so, copyright the Name and make a fortune.'   UNION ALL
    SELECT N'But combinatorial explosion is a harsh master. There are twenty-two Hebrew letters and so 22^36 thirty-six letter Hebrew words. Even with thousands of minimum-wage drones like myself, it takes millions of years to exhaust all of them. That was why you needed to know the rules.

God is awesome in majesty and infinite in glory. He’s not going to have a stupid name like GLBLGLGLBLBLGLFLFLBG. With enough understanding of Adam Kadmon, the secret structure of everything, you could tease out regularities in the nature of God and constrain the set of possible Names to something almost manageable, then make your drones chant that manageable set. This was the applied kabbalah, the project of some of the human race’s greatest geniuses.'
    
 
)
, SortedAnswers(Text, StatusId, RowNum) as  -- This code will generate Sorted Names
(
   /* Number of records from this Select will be: Number of Male records in FirstName WITH clause Multiplied By Number of records IN LastNames WITH clause*/
    select
           Text
         , StatusId
         , ROW_NUMBER() over (Order by newid())
    from Statuses
    cross join Texts
    where StatusId = 1
   UNION
   /* Number of records from this Select will be: Number of Female records in FirstName WITH clause Multiplied By Number of records IN LastNames WITH clause*/
    select
           Text
         , StatusId
         , ROW_NUMBER() over (Order by newid())
    from Statuses
    cross join Texts
    where StatusId = 2
),
Answers AS
(
    select Text
       , StatusId
    from SortedAnswers
)
, Multiplier as
 (
   SELECT 1 N
   UNION ALL
   SELECT N+1
   FROM Multiplier
   WHERE N < @max
 )
INSERT INTO [dbo].[AnswerQueue](AnswerStatusId,Text,CreatedDate,ModifiedDate)
select 
	StatusId,Text, DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 36500), '2020-1-1') CreatedDate, DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 36500), '2020-1-1') ModifiedDate /* Generate random dates between 2020-1-1 and 2140-01-01; 36500 is around 100 years*/
from Answers
cross join Multiplier /*To cross multiply Names WITH clause that returns 198 random {FirstName LastName with gender} combinations with the number that we specify in @max parameter*/
order by NEWID()
option (maxrecursion 0);