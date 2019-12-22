# UTW-Project round 11

### Requirements:
- Registration: User has to register to have access to the system trading and administrating functionality.
  1. To Register must user must enter his name , email , Username and password (Encrypted)
  2. Also to complete the registration user chooses a question from a list of question defined in the system and give the answer (Encrypted) of this question. This question is used when the user forgets the password, to verify the user.

- Login: This phase is used to verify the user, to give him access to the system trading functionality and also the administration functionality in case that the user is admin user.
  1. Login is through username and password
  2. User has the ability to status that he forget his password
  3. User has 3 changes to enter invalid username and password
  4. After 3 failure, user will get 2 extra chances but with Captcha. If the user wasted these 2 chances by entering invalid username, password or Captcha value, user will be locked.

- Forget Password: User is able to inform the system that he forgot his password. The system will give him the ability to enter new password
  1. System will send user an email that include a link to a system page that ask the user for the new password and also ask for the answer for the question he chosen while registering. If answer matches the entered while registration, his password will be changed.
  2. Link sent in email will expire after one day.
  3. Link expires right when user navigate to this link
  
- Order Transaction: responsible for Buying and selling stock for the user
  1. User have the ability add new orders and also update old ones
  2. User has the ability to update orders entered on the same day.
  3. To enter new order user have to choose the stock he wants to buy or sell and the quantity. (User sell according to what he bought)
  4. User have the ability to search for a specific order through Order ID
  
-Monitor Transactions: display all the orders.
  1. If the logged user is admin user, this page will let him have access to all the users’ transaction. Otherwise the page will show the logged user transactions only.
  2. User have the ability to open the details of the order transaction in popup window
  3. The user has the availability to filter the orders with
    a. from date and to date
    b. stock
    c. user (only for admin user)
- Dashboard: Hold system statistics(users only)
  1. This is the first view after login.
  2. No buttons to get the page data. It is loaded when page is opened.
  3. Displays Chart of stocks, Stock Prices and history of Transaction
    a. Chart of stocks will display a pie chart for each stock with the total Transaction Value (Quantity * Market Price) for the logged user.
    b. Stock Price will display the stocks Price.
    c. List of the user transaction of today.
  4. User can navigate to order page from the order list
- Users: This page shows all the users of the system(admin only)
  1. Displays the name , email and status (Active or Blocked) per user
  2. The Logged user can active the Locked user from this page.
  3. User can filter users with
    a. Status (Active Or Locked)
    b. Email
    c. Username


### Snapshots of some pages of the Website

![Alt text](/dashboard.PNG?raw=true "2019 update")
<br/><br/>

![Alt text](/utwAdmin.PNG?raw=true "2019 update")
