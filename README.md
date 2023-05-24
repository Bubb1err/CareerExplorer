# CareerExplorer
  
# Description  
The project is a platform where recruiters and job seekers can find each other.

The project consists of three modules:

1)Recruiters can post vacancies, see users who applied to the vacancies, open a dialog in case of interest and send invitation to meeting.

2)Job seekers can fill out their profiles with their CVs and contact information, and apply for vacancies. 

3)Admin can approve or reject profiles, vacancies, add tags(types of vacancies), tags for requirements(skills, english level).
There is a service that sends vacancies recommendations via email, and also all messages and notificatios are sent into email.

# Database

| Admin |       
| -------- |    
| Id (PK, int, not null)  | 
| UserId (nvarchar, not null)  |

| Recruiter |
| -------- | 
| Id (PK, int, not null)  |
| Name (nvarchar, not null)  |
| Surname (nvarchar, not null)  |
| Company (nvarchar, not null)  |
| CompanyDescription (nvarchar, not null)  |
| IsFilled (bit, not null) |
| IsAccepted (bit, not null) |
| UserId (nvarchar, not null)  |

| JobSeeker |
| -------- | 
| Id (PK, int, not null)  | 
| Name (nvarchar, null)  |
| Surname (nvarchar, null)  |
| Salary (int, null) |
| Phone (nvarchar, null)  |
| Github (nvarchar, null)  |
| LinkedIn (nvarchar, null)  |
| EnglishLevel (int, null) |
| ExperienceYears (int, null) |
| Experience (nvarchar, null)  |
| DesiredPositionId (int, null) |
| IsFilled (bit, not null) |
| IsAccepted (bit, not null) |
| Views (int, null) |
| UserId (nvarchar, not null)  |
| CountryId (int, null) |
| CityId (int, null) |
| IsSubscribedToNotification (bit, not null) |

| Vacancy |
| -------- | 
| Id (PK, int, not null)  | 
| IsAccepted (bit, not null) |
| PositionId (FK, int, not null) |
| Description (nvarchar, not null)  |
| IsAvailable (bit, not null) |
| CreatorId (FK, int, not null) |
| CreatedDate (datetime2, not null) |
| CountryId (int, null) |
| CityId (int, null) |
| Salary (int, null) |
| WorkType (int, null) |
| EnglishLevel (int, null) |
| ExperienceYears (int, null) |
| Views (int, not null) |

| SkillTag |
| -------- | 
| Id (PK, int, not null)  | 
| Title (nvarchar, not null)  |

| Position |
| -------- | 
| Id (PK, int, not null)  | 
| Name (nvarchar, not null)  |

| Message |
| -------- | 
| Id (PK, int, not null)  |
| Text (nvarchar, not null)  |
| TimeSent (datetime2, not null) |
| SenderId (nvarchar, not null)  |
| ReceiverId (nvarchar, not null)  |
| ChatId (FK, int, not null)  |

| Chat |
| -------- | 
| Id (PK, int, not null)  |

| City |
| -------- | 
| Id (PK, int, not null)  | 
| Name (nvarchar, not null)  |
| CountryId (FK, int, not null)  |

| Country |
| -------- | 
| Id (PK, int, not null)  | 
| Name (nvarchar, not null)  |

| AppUserChat |
| -------- | 
| ChatsId (PK, int, not null)  | 
| UsersId (PK, int, not null)  | 

| MeetingNotification |
| -------- | 
| Id (PK, int, not null)  | 
| SenderId (nvarchar, null)  |
| ReceiverId (nvarchar, null)  |
| MeetingLink (nvarchar, null)  |
| IsAccepted (bit, not null) |
| Date (datetime2, not null) |

| JobseekrVacancy |
| -------- | 
| Id (PK, int, not null)  | 
| JobSeekerId (FK, int, not null)  | 
| VacancyId (FK, int, not null)  | 
| Cv (var binary, not null) |
| IsApplied (bit, not null) |

| JobSeekerSkillsTag |
| -------- |
| JobSeekersId (FK, int, not null)  | 
| SkillsId (FK, int, not null)  |

| SkillsTagVacancy |
| -------- |
| RequirementsId (FK, int, not null)  | 
| VacancyId (FK, int, not null)  | 
