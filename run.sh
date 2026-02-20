#!/bin/bash

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"

# Kill anything already on these ports
lsof -ti:7108,5048,5092 | xargs kill -9 2>/dev/null

# Start API
cd "$SCRIPT_DIR/RoyalVilla"
dotnet run --launch-profile https &
API_PID=$!
echo "API started (PID: $API_PID) on https://localhost:7108"

# Wait for API to be ready
echo "Waiting for API to start..."
until curl -sk https://localhost:7108/api/villa > /dev/null 2>&1; do
  sleep 1
done
echo "API is ready!"

# Start Web App
cd "$SCRIPT_DIR/RoyalVillaWeb"
dotnet run &
WEB_PID=$!
echo "Web app started (PID: $WEB_PID) on http://localhost:5092"

echo ""
echo "Both projects running. Open http://localhost:5092 in your browser."
echo "Press Ctrl+C to stop all."
wait $API_PID $WEB_PID
