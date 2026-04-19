-- Fix Customer_Address column size
-- Current: varchar(50) - too small for real addresses
-- New: varchar(500) - should handle full addresses with street, city, province, postal code

ALTER TABLE customer ALTER COLUMN Customer_Address varchar(500) NOT NULL;
