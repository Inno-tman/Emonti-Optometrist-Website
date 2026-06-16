# Emonti Optometrist Website — Test Cases

## Notation
- **Role:** None = public, C = Customer, S = Staff, A = Admin
- **Pre:** Preconditions
- **Steps:** Test steps
- **Expected:** Expected outcome

---

## 1. Authentication

### 1.1 Customer Registration
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| REG-01 | Register valid customer | None | None | 1. Navigate to `/Register`<br>2. Fill all required fields (name, surname, DOB, gender, email, phone, password)<br>3. Fill address via autocomplete or manually<br>4. Submit | Account created. Redirected to `/Login` with success message |
| REG-02 | Duplicate email | None | Email already registered | 1. Register with existing email<br>2. Submit | Error: email already in use |
| REG-03 | Underage (12) | None | None | 1. Set DOB to 12 years ago<br>2. Submit | Error: must be 13+ |
| REG-04 | Invalid phone (non-SA) | None | None | 1. Enter phone not starting with 0, <10 digits<br>2. Submit | Error: invalid phone format |
| REG-05 | Password too short (<6) | None | None | 1. Enter password of 4 chars<br>2. Submit | Error: password must be 6-8 chars |
| REG-06 | Password too long (>8) | None | None | 1. Enter password of 10 chars<br>2. Submit | Error: password must be 6-8 chars |
| REG-07 | Main member not required | None | None | 1. Check "I am main member"<br>2. Leave main member fields blank<br>3. Submit | Account created successfully |
| REG-08 | Non-main member requires ID | None | None | 1. Uncheck "I am main member"<br>2. Leave main member ID blank<br>3. Submit | Error: main member ID required |
| REG-09 | Invalid postal code | None | None | 1. Enter 3-digit postal code<br>2. Submit | Error: postal code must be 4 digits |
| REG-10 | Address autocomplete (OSM) | None | None | 1. Type partial address in SA<br>2. Wait for debounce<br>3. Select suggestion | Address fields populated from Nominatim |
| REG-11 | Blank required fields | None | None | 1. Submit empty form | Validation errors on all required fields |

### 1.2 Customer Login
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| LOG-01 | Valid customer login | None | Registered customer | 1. Go to `/Login`<br>2. Enter correct email + password<br>3. Submit | Redirect to `/Index`. Customer dropdown shows name |
| LOG-02 | Invalid password | None | None | 1. Enter valid email + wrong password<br>2. Submit | Error: "attempt(s) remaining" |
| LOG-03 | Lockout after 5 failures | None | None | 1. Attempt login 5 times with wrong password | After 5th: "Account is locked. Try again after N minutes." |
| LOG-04 | Lockout expiry | None | Locked out | 1. Wait 5 minutes<br>2. Login with correct credentials | Login succeeds |
| LOG-05 | Archived customer | None | Customer with `Is_Archive=1` | 1. Login with that email+password | Error: invalid credentials |

### 1.3 Staff/Admin Login
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| LOG-06 | Staff login | None | Staff account | 1. Login with `staff@emonti.com` / `Staff` | Redirect to `/Staff/Dashboard` |
| LOG-07 | Admin login | None | Admin account | 1. Login with `admin@emonti.com` / `Admin` | Redirect to `/Admin/Dashboard` |
| LOG-08 | Staff login wrong role | None | None | 1. Enter customer email/password<br>2. Submit | Customer login, not staff |

### 1.4 Logout
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| LOG-09 | Customer logout | C | Logged in | 1. Click dropdown → Logout | Redirected to `/Index`. Login/Register buttons visible |
| LOG-10 | Staff logout | S | Logged in | 1. Click dropdown → Logout | Redirected to `/Index`. Staff nav hidden |

### 1.5 Password Reset
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| LOG-11 | Send reset code | None | Registered customer | 1. Click "Forgot Password"<br>2. Enter registered email<br>3. Submit | If SMTP configured: email sent. Otherwise: message shown |
| LOG-12 | Invalid reset code | None | Code sent | 1. Enter wrong 6-digit code<br>2. Submit new password | Error: invalid code |
| LOG-13 | Expired reset code | None | Code sent >15 min ago | 1. Wait 15+ minutes<br>2. Enter code + new password | Error: code expired |
| LOG-14 | Successful reset | None | Valid code in session | 1. Enter correct code + new password<br>2. Confirm | Password updated. Login with new password succeeds |

---

## 2. Shop

### 2.1 Browse Products
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| SHP-01 | View all products | None | None | 1. Go to `/Shop` | All 35 products displayed with name, brand, price, image, stock badge |
| SHP-02 | Filter by category | None | None | 1. Select "Lenses" filter | Only lens products shown |
| SHP-03 | Filter by brand | None | None | 1. Select "Ray-Ban" filter | Only Ray-Ban products shown |
| SHP-04 | Search by name | None | None | 1. Enter "Transitions" in search | Matching products shown |
| SHP-05 | Sort by price asc | None | None | 1. Select "Price: Low to High" | Products sorted ascending by price |
| SHP-06 | Sort by price desc | None | None | 1. Select "Price: High to Low" | Products sorted descending by price |
| SHP-07 | Combined filter + search | None | None | 1. Select "Sunglasses" + enter "Ray-Ban" | Ray-Ban sunglasses only |
| SHP-08 | Out of stock badge | None | Product with `QuantityOnHand=0` | 1. Browse shop | "Out of Stock" badge shown on that product |
| SHP-09 | Limited stock badge | None | Product with `1<=qty<=5` | 1. Browse shop | "Low Stock" or quantity shown |

### 2.2 Product Details
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| SHP-10 | View product | None | None | 1. Click on a product | Product detail page shows full info, description, related products, quantity selector, add-to-cart button |
| SHP-11 | Related products | None | None | 1. View product detail | Products in same category shown in "Related Products" section |
| SHP-12 | Out of stock on detail | None | Product with qty=0 | 1. View detail | Add-to-cart disabled or hidden |

### 2.3 Add to Cart (Guest)
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| SHP-13 | Guest add to cart | None | Not logged in | 1. Click "Add to Cart" on any product | Cart count badge increments. Item stored in session |
| SHP-14 | Guest add same item twice | None | Not logged in | 1. Add product A<br>2. Add product A again | Cart shows quantity 2 for that item |
| SHP-15 | Guest add when stock insufficient | None | Stock=1 | 1. Add until no stock left | Error: "Only N available" or prevented |

### 2.4 Add to Cart (Logged-In Customer)
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| SHP-16 | Customer add to cart | C | Logged in | 1. Click "Add to Cart" | Cart count increments. Item in DB cart |
| SHP-17 | Guest cart merges on login | C | Items in guest session | 1. Add items as guest<br>2. Login | Guest items merged into permanent cart |
| SHP-18 | Add multiple qty from detail | C | Logged in | 1. On product detail, set qty=3<br>2. Click Add to Cart | 3 units added |

### 2.5 Wishlist
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| SHP-19 | Add to wishlist | C | Logged in | 1. Click heart icon on shop/product | Heart fills. Item in wishlist |
| SHP-20 | Remove from wishlist | C | Item in wishlist | 1. Click filled heart icon | Heart unfills. Item removed |
| SHP-21 | Wishlist not logged in | None | Not logged in | 1. Click heart icon | Prompted to login |
| SHP-22 | Duplicate prevention | C | Already in wishlist | 1. Click heart again | Item removed (toggle) |

---

## 3. Cart

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| CRT-01 | View cart | None | Items in cart | 1. Click cart icon | Cart page shows items, quantities, prices, total |
| CRT-02 | Update quantity | None | Item with qty=1 | 1. Click "+" | Quantity increments |
| CRT-03 | Decrease quantity | None | Item with qty>1 | 1. Click "-" | Quantity decrements |
| CRT-04 | Remove item (qty→0) | None | Item with qty=1 | 1. Click "-" to reach 0 | Item removed from cart |
| CRT-05 | Remove item button | None | Any item | 1. Click remove/trash icon | Item removed immediately |
| CRT-06 | Empty cart | None | No items | 1. Visit `/Cart` | Empty cart message with link to shop |
| CRT-07 | Cart count badge | None | N items | 1. Add items | Badge shows correct count |
| CRT-08 | Guest cart persists | None | Not logged in | 1. Add items as guest<br>2. Close browser<br>3. Reopen site | Cart empty (session-based, lost on new session) |

---

## 4. Checkout

### 4.1 Checkout Form
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| CHK-01 | Access checkout with items | C | Items in cart | 1. Click "Checkout" from cart | Checkout form pre-filled with customer address |
| CHK-02 | Empty cart redirect | None | Cart empty | 1. Go to `/Checkout` | Redirected to `/Shop` |
| CHK-03 | Missing required fields | C | Items in cart | 1. Leave email/phone blank<br>2. Select payment method<br>3. Submit | Validation error |
| CHK-04 | Promo code SAVE10 | C | Items in cart, total >0 | 1. Enter "SAVE10" in promo field<br>2. Apply | 10% discount applied |
| CHK-05 | Invalid promo code | C | Items in cart | 1. Enter "INVALID"<br>2. Apply | Error: invalid promo code |

### 4.2 Cash on Delivery
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| CHK-06 | Place COD order | C | Items in cart, address filled | 1. Select "Cash on Delivery"<br>2. Submit | Order created with "Pending" status. Redirected to Order Confirmation page. Inventory decremented. Cart cleared. |
| CHK-07 | COD payment record | C | After COD order | 1. Check Payments table | Payment record with `Payment_Status='Pending'`, `Amount_Received=0` |

### 4.3 Paystack Credit Card
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| CHK-08 | Open Paystack modal | C | Items in cart, address filled | 1. Select "Credit/Debit Card"<br>2. Submit | Paystack inline overlay opens with correct amount |
| CHK-09 | Successful payment | C | Valid test card | 1. Enter Paystack test card (4084 0810 0000 0030, any CVV, any future date)<br>2. Pay | Redirected to `/OrderConfirmation?ref=paystackref&id=N`. Order status = "Processing". Payment status = "Paid" |
| CHK-10 | Failed payment | C | None | 1. Enter Paystack failure test card (4000 0000 0000 0002)<br>2. Pay | Paystack shows failure. No order created. Customer stays on checkout |
| CHK-11 | Paystack verification fallback | C | `SecretKey` not set | 1. Complete Paystack flow | OrderConfirmation loads. Shows success but Paystack verification skipped. Order stays "Pending" |

### 4.4 Order Confirmation
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| CHK-12 | View confirmation | C | Order just placed | 1. Redirect to OrderConfirmation | Order number, date, items, total, delivery address, payment method all displayed |
| CHK-13 | Direct access without ref | C | Order exists | 1. Go to `/OrderConfirmation?id=N` | Order details displayed normally |

---

## 5. Appointment Booking

### 5.1 Business Rules
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| BOK-01 | Not logged in | None | Not logged in | 1. Go to `/BookAppointment` | Redirect to `/Login` |
| BOK-02 | Past date | C | Logged in | 1. Select yesterday's date | Error: "Please select a future date" |
| BOK-03 | Sunday | C | Logged in | 1. Select a Sunday | Error: "We are closed on Sundays" |
| BOK-04 | Saturday after 14:00 | C | Logged in | 1. Select Saturday at 15:00 | Slot not available |
| BOK-05 | Business hours | C | Logged in | 1. Select Monday at 07:00 | Slot not available |
| BOK-06 | Two appointments same day | C | One appointment exists on date | 1. Book another on same date | Error: "one appointment per day" |
| BOK-07 | Double-booked optometrist | C | Optometrist already booked that slot | 1. Select same optometrist, date, time | Error: slot unavailable |
| BOK-08 | Blocked timeslot | C | Staff blocked a slot | 1. Try to book that slot | Error: slot unavailable |
| BOK-09 | Same-day 2-hour rule | C | Current time 10:00, want 11:00 | 1. Book for today at 11:00 | Error: "requires at least 2 hours" |
| BOK-10 | Successful booking | C | All rules pass | 1. Select future date, time, optometrist<br>2. Submit | Appointment created with "Pending" status. Confirmation email sent (if SMTP configured). |

### 5.2 Rebooking
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| BOK-11 | Rebook from cancelled | C | Cancelled appointment | 1. Click rebook link with `?rebook=ID` | Form pre-filled with previous optometrist, date, time |
| BOK-12 | Rebook invalid ID | C | None | 1. Go to `/BookAppointment?rebook=99999` | New blank form (no crash) |

---

## 6. Appointments (Customer View)

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| APT-01 | View appointments | C | Has appointments | 1. Go to `/Appointments` | Appointments listed: Upcoming, Past, Cancelled with color-coded status |
| APT-02 | Empty state | C | No appointments | 1. Go to `/Appointments` | "No Appointments Found" with link to book |
| APT-03 | Cancel >2h before | C | Upcoming appointment >2h away | 1. Click "Cancel"<br>2. Confirm | Status changes to "Cancelled" |
| APT-04 | Cancel within 2h | C | Appointment within 2h | 1. Check page | Cancel button hidden |
| APT-05 | Cancel already cancelled | C | Already cancelled | 1. Click Cancel | Error: "already cancelled" |
| APT-06 | Cancel past appointment | C | Past appointment | 1. Check page | Cancel button hidden |

---

## 7. Staff Dashboard

### 7.1 Dashboard Overview
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| STA-01 | Access dashboard | S | Staff logged in | 1. Go to `/Staff/Dashboard` | KPI cards: today's appointments, pending orders, total patients, upcoming appointments |
| STA-02 | Today's appointments | S | Appointments exist | 1. View dashboard | Today's appointments listed with patient name, time, phone |
| STA-03 | Cancel appointment | S | Future appointment exists | 1. Click Cancel on an appointment | Appointment cancelled. Success message shown |

### 7.2 Timeslot Management
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| STA-04 | View calendar | S | Staff logged in | 1. Scroll to timeslot section | 30-day calendar shown. Clicking a date loads timeslots |
| STA-05 | View timeslots for a date | S | Staff logged in | 1. Click a date | Time grid shows: green(available), yellow(blocked), red(booked) |
| STA-06 | Block a slot | S | Slot is available | 1. Click a green slot | Turns yellow. BlockedTimeslots record created |
| STA-07 | Unblock a slot | S | Slot is blocked | 1. Click a yellow slot | Turns green. BlockedTimeslots record deleted |
| STA-08 | Block booked slot | S | Slot is booked (red) | 1. Click a red slot | No change (already booked) |
| STA-09 | Customer cannot book blocked | S/C | Staff blocked a slot | 1. As customer, try to book that slot | Slot shown as unavailable |

---

## 8. Messaging

### 8.1 Customer Messages
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| MSG-01 | Access messages | C | Logged in | 1. Click dropdown → Messages | `/Messages` page shows empty state or previous conversations |
| MSG-02 | New message | C | Logged in | 1. Enter subject + body<br>2. Click Send | Conversation created. View changes to thread |
| MSG-03 | View thread | C | Has conversation | 1. Click a conversation | Thread displays all messages, newest at bottom |
| MSG-04 | Reply to thread | C | Conversation exists | 1. Type reply + Send | Reply added to thread |
| MSG-05 | Unread badge (customer) | C | Staff has replied | 1. Look at conversation list | Unread badge number shows on that thread |
| MSG-06 | Mark read (customer) | C | Unread staff reply | 1. Click the conversation | Badge disappears. Messages marked read |

### 8.2 Staff Messages
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| MSG-07 | Access staff messages | S | Staff logged in | 1. Click dropdown → Messages | `/Staff/Messages` lists all customer conversations |
| MSG-08 | Unread badge (staff) | S | Customer has sent messages | 1. View conversation list | Unread count on each thread |
| MSG-09 | Staff reply | S | Conversation exists | 1. Click thread → Type reply → Send | Reply sent. Customer can see it |
| MSG-10 | Mark read (staff) | S | Unread customer messages | 1. Click the conversation | Badge disappears. Messages marked read |
| MSG-11 | Empty state (staff) | S | No messages | 1. View `/Staff/Messages` | "No Messages Yet" |

---

## 9. Orders (Customer View)

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| ORD-01 | View orders | C | Has orders | 1. Go to `/Orders` | Orders listed with date, status, total, item count |
| ORD-02 | Empty orders | C | No orders | 1. Go to `/Orders` | "No orders" message |
| ORD-03 | Order details | C | Order exists | 1. Click an order | Items, quantities, prices, delivery address displayed |

---

## 10. Admin Pages

### 10.1 Admin Dashboard
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| ADM-01 | Access dashboard | A | Admin logged in | 1. Go to `/Admin/Dashboard` | KPI cards: orders today, revenue, pending orders, products, staff, today's appointments, new customers, recent orders |
| ADM-02 | Staff cannot access | S | Staff logged in | 1. Go to `/Admin/Dashboard` | Redirected to `/Login` |

### 10.2 Manage Orders
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| ADM-03 | View all orders | A/S | Orders exist | 1. Go to `/Admin/ManageOrders` | All orders listed with customer name, date, total, status |
| ADM-04 | Filter by status | A/S | Mixed status orders | 1. Select "Pending" filter | Only pending orders shown |
| ADM-05 | Search orders | A/S | Orders exist | 1. Enter order number or customer name | Matching orders shown |
| ADM-06 | Update order status | A/S | Order exists | 1. Select new status from dropdown | Status updated immediately. Badge changes |

### 10.3 Manage Products
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| ADM-07 | View products | A | Admin logged in | 1. Go to `/Admin/ManageProducts` | All products listed with edit/delete buttons |
| ADM-08 | Add product | A | None | 1. Fill new product form (name, brand, category, price, stock, image)<br>2. Submit | Product added to list |
| ADM-09 | Edit product | A | Product exists | 1. Click Edit on a product<br>2. Change fields<br>3. Save | Product updated |
| ADM-10 | Delete product | A | Product exists | 1. Click Delete<br>2. Confirm | Product removed from list |

### 10.4 Manage Staff
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| ADM-11 | View staff | A | Admin logged in | 1. Go to `/Admin/ManageStaff` | All staff listed with role, remove/promote buttons |
| ADM-12 | Add staff | A | None | 1. Fill name, email, surname, password, role<br>2. Submit | Staff added with GUID ID |
| ADM-13 | Duplicate staff email | A | Staff with email exists | 1. Try to add same email again | Error: email already used |
| ADM-14 | Remove staff (not admin) | A | Staff account exists | 1. Click Remove on non-admin staff | Staff deleted |
| ADM-15 | Cannot remove admin | A | Admin account exists | 1. Click Remove on admin | Error or no change |
| ADM-16 | Promote to admin | A | Staff account exists | 1. Click "Make Admin" on a staff member | Role changed to Admin |

### 10.5 Manage Customers
| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| ADM-17 | View customers | A | Admin logged in | 1. Go to `/Admin/ManageCustomers` | All customers listed |
| ADM-18 | Search customers | A | Customers exist | 1. Type name/email in search | Matching customers shown |
| ADM-19 | Customer detail expand | A | Customer has orders | 1. Click expand on a customer row | Order history shown |

---

## 11. Personal Details

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| PRF-01 | View details | C | Logged in | 1. Go to `/PersonalDetails` | Form pre-filled with customer's current info |
| PRF-02 | Update name | C | Logged in | 1. Change first name<br>2. Submit | Name updated. Display reflects change |
| PRF-03 | Update address | C | Logged in | 1. Change street/city/postal code<br>2. Submit | Address updated in `Customer_Address` column |
| PRF-04 | Invalid update | C | Logged in | 1. Enter invalid phone<br>2. Submit | Validation error |

---

## 12. Reports

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| RPT-01 | View reports | S/A | Staff/Admin logged in | 1. Go to `/Reports` | KPIs: total orders, revenue, products sold, customer count. Recent orders + popular products tables |
| RPT-02 | Customer cannot access | C | Logged in | 1. Go to `/Reports` | Redirected or blocked |
| RPT-03 | Student cannot access | None | Not logged in | 1. Go to `/Reports` | Redirected to login |

---

## 13. FAQ Chatbot

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| CHT-01 | Open chatbot | None | None | 1. Click FAQ Assistant button | Chat window opens |
| CHT-02 | Keyword match FAQ | None | FAQ exists | 1. Type "hours" or "opening time" | Returns: "We are open Monday to Friday..." |
| CHT-03 | AI response (if configured) | None | Groq API key set | 1. Type unique question not in FAQ | AI-generated response |
| CHT-04 | Fallback response | None | No match, no AI | 1. Type gibberish | "Please contact us directly..." |
| CHT-05 | Logged conversation | None | None | 1. Ask any question | Conversation logged to `Chat_Conversations` |
| CHT-06 | Close chatbot | None | Chat open | 1. Click X button | Chat window closes |

---

## 14. Navigation & Layout

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| NAV-01 | Public nav | None | Not logged in | 1. View top nav | Home, Services, Shop, Book Appointment, About, Contact, Help |
| NAV-02 | Customer dropdown | C | Logged in | 1. Click avatar/dropdown | Personal Details, Orders, Appointments, Messages, Wishlist, Logout |
| NAV-03 | Staff dropdown | S | Staff logged in | 1. Click dropdown | Staff header, Dashboard, Messages, Logout |
| NAV-04 | Admin nav | A | Admin logged in | 1. View page | Staff nav (View Report) + Admin dropdown with Dashboard, Manage Orders, Manage Products, Manage Staff, Messages, Logout |
| NAV-05 | Cart badge | None | Items in cart | 1. Add items | Badge count visible on cart icon |
| NAV-06 | Mobile menu | None | Narrow viewport | 1. Resize to mobile | Hamburger menu appears. Nav collapses |

---

## 15. Edge Cases

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| EDG-01 | Concurrent same-stock purchase | C | Stock=1, 2 customers | 1. Both add to cart<br>2. Both checkout simultaneously | One succeeds, one gets insufficient stock error |
| EDG-02 | Double-booking prevention race | C | Same optometrist/time | 1. Two customers book same slot simultaneously | One succeeds, one gets conflict error |
| EDG-03 | SQL injection attempt | None | None | 1. Enter `'; DROP TABLE customer;--` in any text field | Treated as literal string, no injection |
| EDG-04 | XSS attempt | None | None | 1. Enter `<script>alert(1)</script>` in message/register | Rendered as text, not executed |
| EDG-05 | Session timeout | C | Idle for session timeout | 1. Wait > session duration<br>2. Try to access customer page | Redirected to login |
| EDG-06 | Direct URL access (no auth) | None | Not logged in | 1. Try `/Admin/Dashboard`, `/Staff/Dashboard`, `/Orders`, etc. | Redirected to `/Login` |
| EDG-07 | Browser back after order | C | Order placed | 1. Press browser back from confirmation | Should not re-submit order |

---

## 16. Database Schema Integrity

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| DB-01 | App init creates tables | None | Fresh app.db | 1. Delete app.db<br>2. Start app | All 16 tables created with correct schema |
| DB-02 | Seed data | None | Fresh database | 1. After init | 35 products, 2 staff (admin+staff), 10 FAQs, 16 time slots, admin/staff seeded |
| DB-03 | Last_Login migration | None | Existing DB without column | 1. Start app with old DB | Column added, backfilled |
| DB-04 | BlockedTimeslots Created_At migration | None | Existing DB without column | 1. Start app with old DB | Column added |

---

## 17. Staff Nav: View Report Link

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| NAV-07 | View Report link visible | S/A | Staff or Admin logged in | 1. View nav bar | "View Report" link appears between nav and cart for staff/admin |
| NAV-08 | View Report link hidden | C/None | Customer or guest | 1. View nav bar | "View Report" link is absent |

---

## 18. Admin-Only Access Enforcement

| ID | Scenario | Role | Pre | Steps | Expected |
|----|----------|------|-----|-------|----------|
| SEC-01 | ManageStaff admin-only | S | Staff logged in | 1. Go to `/Admin/ManageStaff` | Redirected to `/Login` |
| SEC-02 | ManageProducts admin-only | S | Staff logged in | 1. Go to `/Admin/ManageProducts` | Redirected to `/Login` |
| SEC-03 | ManageCustomers admin-only | S | Staff logged in | 1. Go to `/Admin/ManageCustomers` | Redirected to `/Login` |
| SEC-04 | Staff can access ManageOrders | S | Staff logged in | 1. Go to `/Admin/ManageOrders` | Page loads (staff also allowed) |
