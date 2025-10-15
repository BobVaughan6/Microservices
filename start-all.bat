@echo off
echo Starting Microservices System...
echo ================================
echo.

echo Starting User Service on port 5001...
start "User Service" cmd /k "cd src\UserService\UserService && dotnet run"

timeout /t 2 /nobreak >nul

echo Starting Product Service on port 5002...
start "Product Service" cmd /k "cd src\ProductService\ProductService && dotnet run"

timeout /t 2 /nobreak >nul

echo Starting API Gateway on port 5000...
start "API Gateway" cmd /k "cd src\ApiGateway\ApiGateway && dotnet run"

echo.
echo All services started!
echo ================================
echo API Gateway:      http://localhost:5000
echo User Service:     http://localhost:5001
echo Product Service:  http://localhost:5002
echo.
echo Close the individual windows to stop each service.
echo.
pause
