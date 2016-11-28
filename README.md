# CS430 Project

**Description**: I have written this project in C# using ASP.NET Core MVC as a backend.

**Challenges**: Due to the requirements, SSN is considered a primary key. However, because of the limitations 
of ASP.NET Core, PKs are not editable with the schema I have used. Therefore, I have used surrogate keys 
that act as the unique id for each row while SSN is still included as unique and editable.

Also, since the requirement stated that a code needs to be input by the Notown Staff, I decided that instead of 
incorporating a role-based or policy-based authentication system I would simply require the code be input at time 
of account creation. Then just assume every authenticated user is a staff member and obfuscate data accordingly. 
Obviously, in a real-world situation, this would not be an appropriate solution.

**Sample User Account**:

user: admin@notownrecords.com

pass: Pass!123

code: cs430@SIUC


# Project Requirements

The company, Notown Records, has decided to store information about musicians who
perform on its albums (as well as other company data) in a database. The company has
wisely chosen to hire you as a database designer (at your usual consulting fee of
$2500/day!).

1. Each musician that records at Notown has an SSN, a name, an address, and a
phone number. Poorly paid musicians often share the same address, and no
address has more than one phone.

2. Each instrument used in songs recorded at Notown has a unique identification
number, a name (e.g., guitar, synthesizer, and flute) and a musical key (e.g., C, Bflat,
E-flat).

3. Each album recorded on the Notown label has a unique identification number, a title,
a copyright date, a speed.

4. Each song recorded at Notown has a title and an author.

5. Each musician may play several instruments, and a given instrument may be played
by several musicians.

6. Each album has a number of songs on it, but no song may appear on more than one
album.

7. Each song is performed by one or more musicians, and a musician may perform a
number of songs.

8. Each album has exactly one musician who acts as its producer. A musician may
produce several albums, of course.

To model the situations described above, your colleague has designed a conceptual
database schema, which is illustrated by the ER diagram of Figure 1 (Note that the
convention in the figure is slightly different from our class examples: a heavy weight
arrow pointing from a weak entity set to its supporting relationship, which further
connects to the owner entity set).

## Now you are required to do the following:
1. Review the conceptual design made by your colleague (as delineated above) to see if there
are any constraints as indicated/implied in the situations described above that are not yet
captured in the conceptual design.

2. Review the preceding SQL statements deigned by your colleague to see if any constraints are
not captured in these schemas. If you do find some, make sure you implement them either as
triggers or have them coded into your programs (in any way, you shall not modify the given
schema, i.e., the given SQL statements).

3. Create an Oracle database based on the given schema and develop the application programs
using Java to fulfill the following specific requirements:

    a. Customers can search for records by name of the musician, title of the album, and
name of the song.

    b. Notown staff can search any table and can add/delete/alter a row in any table. Each
Notown staff member has to provide this security code, cs430@SIUC, before she/he
can submit an update command. No security check is needed for pure search. 

    c. Notown requires a user-friendly GUI. Items (a) and (b) shall be nicely integrated into
this GUI environment.

    d. For test purpose, please populate each table with at least five simulated (look-likereal)
records/rows.


## Important Notes:
1. With regard to requirements 1 and 2, if you find any constraints not represented in the
given schemas, try to enforce them either via additional triggers or appropriate code in
your application programs.

2. The given schema (SQL statements) my not fully comply with the Oracle dialect of SQL.
You need to adapt it to Oracle SQL (There are links at the webpage of this course that
lead to useful Oracle resources).

3. The TA will work out submission guideline (including specifics on what you shall
include in your submission package); remember to check the guideline or ask about it
before you make your final submission.

4. My most candid suggestion to all students is to start early as there are always some
issues that are unexpected and hinder your progress.

5. You may earn up to 10 bonus points for a professionally completed work (besides
fulfilling all the described requirements which shall be considered as minimum
requirements).

6. Hints to get bonus: Take this project as a real-world project, be thoughtful and
creative. As a professional, your thoughts and final implementation shall reasonably go
beyond simple fulfilling the specified minimum requirements in order to most or all of
the bonus points.
