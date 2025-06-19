-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-06-03 20:19:43.353

-- tables
-- Table: Event
CREATE TABLE Event (
    Id int  NOT NULL IDENTITY,
    Title varchar(150)  NOT NULL,
    Description varchar(500)  NOT NULL,
    Date datetime  NOT NULL,
    OrganizerId int  NOT NULL,
    CONSTRAINT Event_pk PRIMARY KEY  (Id)
);

-- Table: EventParticipant
CREATE TABLE EventParticipant (
    User_Id int  NOT NULL,
    Event_Id int  NOT NULL,
    CONSTRAINT EventParticipant_pk PRIMARY KEY  (User_Id,Event_Id)
);

-- Table: EventTag
CREATE TABLE EventTag (
    Event_Id int  NOT NULL,
    Tag_Id int  NOT NULL,
    CONSTRAINT EventTag_pk PRIMARY KEY  (Event_Id,Tag_Id)
);

-- Table: Tag
CREATE TABLE Tag (
    Id int  NOT NULL IDENTITY,
    Name varchar(50)  NOT NULL,
    CONSTRAINT Tag_pk PRIMARY KEY  (Id)
);

-- Table: User
CREATE TABLE "User" (
    Id int  NOT NULL IDENTITY,
    Username varchar(50)  NOT NULL,
    Email varchar(150)  NOT NULL,
    CONSTRAINT User_pk PRIMARY KEY  (Id)
);

-- foreign keys
-- Reference: EventParticipant_Event (table: EventParticipant)
ALTER TABLE EventParticipant ADD CONSTRAINT EventParticipant_Event
    FOREIGN KEY (Event_Id)
    REFERENCES Event (Id);

-- Reference: EventParticipant_User (table: EventParticipant)
ALTER TABLE EventParticipant ADD CONSTRAINT EventParticipant_User
    FOREIGN KEY (User_Id)
    REFERENCES "User" (Id);

-- Reference: EventTags_Event (table: EventTag)
ALTER TABLE EventTag ADD CONSTRAINT EventTags_Event
    FOREIGN KEY (Event_Id)
    REFERENCES Event (Id);

-- Reference: EventTags_Tag (table: EventTag)
ALTER TABLE EventTag ADD CONSTRAINT EventTags_Tag
    FOREIGN KEY (Tag_Id)
    REFERENCES Tag (Id);

-- Reference: Event_User (table: Event)
ALTER TABLE Event ADD CONSTRAINT Event_User
    FOREIGN KEY (OrganizerId)
    REFERENCES "User" (Id);

-- End of file.


-- ================================================
-- Sample DML Script for SQL Server
-- Inserts demo data into all 5 tables
-- ================================================

-- Insert Users
INSERT INTO "User" (Username, Email) VALUES
('john_doe', 'john@example.com'),
('jane_smith', 'jane@example.com'),
('alice_wonder', 'alice@example.com');

-- Insert Tags
INSERT INTO Tag (Name) VALUES
('Tech'),
('Education'),
('Health');

-- Insert Events
-- Assuming OrganizerId 1, 2, 3 already exist
INSERT INTO Event (Title, Description, Date, OrganizerId) VALUES
('Tech Conference 2025', 'Annual tech conference covering AI and Cloud topics.', '2025-09-10 09:00:00', 1),
('Health & Wellness Fair', 'Fair promoting healthy lifestyle and wellness.', '2025-07-15 10:00:00', 2),
('Education Summit', 'Summit on modern teaching methods and edtech.', '2025-08-22 11:30:00', 3);

-- Insert Event Participants
-- Format: (User_Id, Event_Id)
INSERT INTO EventParticipant (User_Id, Event_Id) VALUES
(1, 1),
(2, 1),
(3, 2),
(1, 3),
(2, 3);

-- Insert Event Tags
-- Format: (Event_Id, Tag_Id)
INSERT INTO EventTag (Event_Id, Tag_Id) VALUES
(1, 1), -- Tech Conference → Tech
(2, 3), -- Health Fair → Health
(3, 2), -- Education Summit → Education
(1, 2), -- Tech Conference → Education
(3, 1); -- Education Summit → Tech

-- ================================================
-- End of script
-- ================================================

