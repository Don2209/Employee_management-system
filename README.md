# Employee_management-system
Overview
This project is a comprehensive Employee Management System designed for managers to efficiently manage employee records, leave schedules, and performance metrics. The application is built with a TypeScript front-end (React), a C# back-end (ASP.NET Core), and a MySQL database using MySQL Workbench.

Features
Employee Management: Add, edit, view, and delete employee records.
Leave Management: Track leave schedules, including the number of leave days due, taken, and notify employees when leave dates are approaching.
Performance Tracking: Record and manage employee performance metrics such as hours worked and tasks completed.
Report Generation: Extract detailed reports on employee performance, working hours, and other key metrics.

Tech Stack
Front-end: TypeScript, React
Back-end: C# (ASP.NET Core)
Database: MySQL (using MySQL Workbench)


Prerequisites
Node.js: Install from Node.js official website
.NET Core SDK: Install from Microsoft .NET download page
Workbench: Install from MySql Workbench

Installation required for back-end
1.   dotnet new webapi -n EmployeeManagement
2.   cd EmployeeManagement
3.  dotnet add package Microsoft.EntityFrameworkCore
4. dotnet add package Pomelo.EntityFrameworkCore.MySql

Steps for front-end
1. npx create-react-app employee-management --template typescript
2. cd employee-management
3. npm install axios
4. npm install bootstrap
5. npm install react-router-dom



