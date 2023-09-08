import cv2
import numpy as np

# Initialize the camera
camera = cv2.VideoCapture(0)

# Define the dimensions of your rectangular blocks (in pixels)
block_width = 400
block_height = 200

# Calculate the minimum area for valid detection
min_area = block_width * block_height

while True:
    ret, frame = camera.read()
    if not ret:
        break
    
    # Color detection and contour analysis
    gray_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    _, thresholded_frame = cv2.threshold(gray_frame, 150, 255, cv2.THRESH_BINARY)
    contours, _ = cv2.findContours(thresholded_frame, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    
    block_detected = False
    
    for contour in contours:
        area = cv2.contourArea(contour)
        if area > min_area:
            # Check if the contour is approximately rectangular
            perimeter = cv2.arcLength(contour, True)
            approx = cv2.approxPolyDP(contour, 0.04 * perimeter, True)
            
            if len(approx) == 4:
                # Ensure that the contour is a rectangle by checking its aspect ratio
                x, y, w, h = cv2.boundingRect(approx)
                aspect_ratio = float(w) / h
                if 0.8 <= aspect_ratio <= 1.2:
                    block_detected = True
                    # Draw an outline around the detected block
                    cv2.drawContours(frame, [approx], -1, (0, 255, 0), 2)
                    # Display block information
                    cv2.putText(frame, "Block: Rectangle", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)
    
    if not block_detected:
        # Display "No block detected" message
        cv2.putText(frame, "No block detected", (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 1, (255, 0, 0), 2)
    
    cv2.imshow("Block Detection", frame)
    
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

camera.release()
cv2.destroyAllWindows()
