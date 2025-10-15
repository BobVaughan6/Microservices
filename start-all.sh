#!/bin/bash

echo "Starting Microservices System..."
echo "================================"
echo ""

# Start User Service
echo "Starting User Service on port 5001..."
cd src/UserService/UserService
dotnet run &
USER_SERVICE_PID=$!
cd ../../..

# Wait a bit
sleep 2

# Start Product Service
echo "Starting Product Service on port 5002..."
cd src/ProductService/ProductService
dotnet run &
PRODUCT_SERVICE_PID=$!
cd ../../..

# Wait a bit
sleep 2

# Start API Gateway
echo "Starting API Gateway on port 5000..."
cd src/ApiGateway/ApiGateway
dotnet run &
API_GATEWAY_PID=$!
cd ../../..

echo ""
echo "All services started!"
echo "================================"
echo "API Gateway:      http://localhost:5000"
echo "User Service:     http://localhost:5001"
echo "Product Service:  http://localhost:5002"
echo ""
echo "To stop all services, run: kill $USER_SERVICE_PID $PRODUCT_SERVICE_PID $API_GATEWAY_PID"
echo ""
echo "Press Ctrl+C to stop all services"

# Wait for Ctrl+C
trap "echo 'Stopping services...'; kill $USER_SERVICE_PID $PRODUCT_SERVICE_PID $API_GATEWAY_PID; exit" INT
wait
