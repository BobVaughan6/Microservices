#!/bin/bash

echo "Testing Microservices System"
echo "============================"
echo ""

echo "1. Checking Health Status..."
echo "----------------------------"
echo "API Gateway Health:"
curl -s http://localhost:5000/health | python3 -m json.tool
echo ""
echo "User Service Health:"
curl -s http://localhost:5001/health | python3 -m json.tool
echo ""
echo "Product Service Health:"
curl -s http://localhost:5002/health | python3 -m json.tool
echo ""

echo "2. Testing User Service via API Gateway..."
echo "-------------------------------------------"
echo "Getting all users:"
curl -s http://localhost:5000/api/users | python3 -m json.tool
echo ""
echo "Getting user with ID 1:"
curl -s http://localhost:5000/api/users/1 | python3 -m json.tool
echo ""

echo "3. Testing Product Service via API Gateway..."
echo "----------------------------------------------"
echo "Getting all products:"
curl -s http://localhost:5000/api/products | python3 -m json.tool
echo ""
echo "Getting product with ID 2:"
curl -s http://localhost:5000/api/products/2 | python3 -m json.tool
echo ""

echo "4. Testing Direct Service Access..."
echo "------------------------------------"
echo "Accessing User Service directly:"
curl -s http://localhost:5001/api/users/1 | python3 -m json.tool
echo ""
echo "Accessing Product Service directly:"
curl -s http://localhost:5002/api/products/1 | python3 -m json.tool
echo ""

echo "============================"
echo "All tests completed!"
echo "============================"
