# Registration Database Insert Fix

## The Problem

You're getting the error: **"An error occurred while creating your account: Failed to create account"**

This is a generic error message that was hiding the real database error. The issue could be one of several things:

### Common Causes:
1. **Empty string vs NULL handling** - Database expects NULL for optional fields, but code sends empty strings
2. **Column data type mismatches** - String being inserted into numeric field, etc.
3. **Required columns missing values** - Database has NOT NULL columns that aren't being filled
4. **Data type length violations** - String too long for column definition
5. **Foreign key constraints** - Reference to non-existent data
6. **Database connection issues** - Network, credentials, or permissions

## What Was Fixed

### 1. Improved Error Reporting
**Before:**
```csharp
catch (Exception Ex)
{
    transaction.Rollback();
    throw new Exception("Failed to create account");  // Generic, unhelpful
}
```

**After:**
```csharp
catch (Exception ex)
{
    transaction.Rollback();
    System.Diagnostics.Debug.WriteLine($"Database insert error: {ex.Message}");
    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
    throw new Exception($"Database error: {ex.Message}", ex);  // Shows actual error
}
```

Now you'll see the **actual database error message** instead of a generic message!

### 2. Proper NULL Handling for Optional Fields
**Before:**
```csharp
cmd.Parameters.AddWithValue("@Medical_Aid", txtMedicalAid.Text.Trim());  // Sends "" if empty
```

**After:**
```csharp
cmd.Parameters.AddWithValue("@Medical_Aid", 
    string.IsNullOrWhiteSpace(txtMedicalAid.Text) ? (object)DBNull.Value : txtMedicalAid.Text.Trim());
```

This ensures that:
- Empty optional fields are sent as `NULL` to the database
- The database can properly handle nullable columns
- No empty string violations for columns expecting NULL

### 3. Better Address Building
Created a dedicated `BuildAddress()` method that:
- Only includes non-empty address components
- Properly formats the address string
- Returns empty string if no address info provided
- Handles NULL for the address field if empty

### 4. Cleaner Code Organization
Separated required vs optional fields with clear comments:
```csharp
// Required fields
cmd.Parameters.AddWithValue("@Customer_Name", txtFirstName.Text.Trim());
cmd.Parameters.AddWithValue("@Customer_Surname", txtLastName.Text.Trim());
cmd.Parameters.AddWithValue("@Customer_Email", txtEmail.Text.Trim());
cmd.Parameters.AddWithValue("@Customer_Phone", txtPhone.Text.Trim());
cmd.Parameters.AddWithValue("@Customer_Password", txtPassword.Text.Trim());

// Optional fields - handle nulls properly
cmd.Parameters.AddWithValue("@Customer_DOB",
    string.IsNullOrWhiteSpace(txtDateOfBirth.Text) ? (object)DBNull.Value : DateTime.Parse(txtDateOfBirth.Text));
```

## Diagnostic Tool Created

I've created `TestDatabaseSchema.aspx` to help diagnose database issues:

### Features:
1. **Test Connection** - Verify database connectivity
2. **Get Schema** - See exact table structure, column types, nullable fields
3. **Test Insert** - Try a minimal insert to identify specific issues

### How to Use:
1. Navigate to: `http://yoursite/TestDatabaseSchema.aspx`
2. Click "Test Database Connection" - Should show green success
3. Click "Get Customer Table Schema" - Shows all columns, types, and whether they accept NULL
4. Click "Test Insert" - Attempts to insert minimal test data

This will show you **exactly** which columns are causing problems!

## How to Debug Your Specific Issue

### Step 1: Check the Database Schema
Run the diagnostic page and check:
- Are all the columns in your INSERT statement actually in the table?
- What data types are they expecting?
- Which columns are NOT NULL (required)?
- What are the maximum lengths for VARCHAR fields?

### Step 2: Check the Actual Error
With the improved error handling, you should now see messages like:
- "Cannot insert NULL value into column 'Customer_Name'" - Required field missing
- "String or binary data would be truncated" - Value too long for column
- "Invalid column name 'XYZ'" - Column doesn't exist in table
- "Violation of PRIMARY KEY constraint" - Duplicate key

### Step 3: Common Fixes

**If column doesn't exist:**
- Check spelling in both ASPX and database
- Verify column exists: `SELECT * FROM customer WHERE 1=0`

**If NULL constraint violation:**
- Make column nullable in database, OR
- Ensure the code always provides a value

**If data truncation:**
- Check maximum lengths in database
- Add validation to prevent over-long inputs

**If duplicate key:**
- Check if email already exists (we have EmailExists() for this)
- Ensure proper unique constraints

## Testing the Fix

### Test Case 1: Minimal Required Fields Only
Fill in ONLY:
- First Name: Test
- Last Name: User
- Email: test@example.com
- Phone: 0123456789
- Password: test123
- Confirm Password: test123

Leave everything else empty. This should work now!

### Test Case 2: All Fields Filled
Fill in every field on the form. This should also work!

### Test Case 3: Some Optional Fields
Fill in required fields + some optional ones (like medical aid). Should work!

## Quick Reference: Required vs Optional Fields

### Required (NOT NULL in database):
- ✓ Customer_Name (First Name)
- ✓ Customer_Surname (Last Name)
- ✓ Customer_Email
- ✓ Customer_Phone
- ✓ Customer_Password

### Optional (Can be NULL):
- Customer_DOB (Date of Birth)
- Customer_Gender
- Customer_Address
- Medical_Aid
- Medical_Aid_Number
- Main_Member_Name
- Main_Member_Surname
- Main_Member_ID
- Street_Number
- Street_Name
- Complex_Name
- Unit_Number
- City
- Province
- Postal_Code

## What to Do Next

1. **Navigate to the registration page** and try to register
2. **If it still fails**, check the browser console and Visual Studio Output window for the actual error message
3. **Run the diagnostic page** to see the exact database schema
4. **Check the error message** - it will now show the real database error
5. **Compare the database schema** with what the code is trying to insert

## Need More Help?

If you still get an error, please provide:
1. The **actual error message** now showing from the database
2. The **database schema** from the diagnostic page (screenshot or text)
3. What **values** you entered in the form

The new error messages will tell us exactly what's wrong!

## Files Modified
- ✅ `Account/Register.aspx.cs` - Better error handling and NULL management
- ✅ `TestDatabaseSchema.aspx` - NEW diagnostic tool
- ✅ `TestDatabaseSchema.aspx.cs` - NEW diagnostic tool code
- ✅ `REGISTRATION_DATABASE_FIX.md` - This documentation

