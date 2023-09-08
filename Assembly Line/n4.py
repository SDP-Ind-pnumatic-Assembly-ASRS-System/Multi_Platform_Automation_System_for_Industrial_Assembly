import cv2
import numpy as np

min_area_threshold = 100  # Adjust based on your application and camera resolution
max_aspect_ratio = 0.8    # Adjust based on the expected shape of holes


def detect_holes(frame):
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    blurred = cv2.GaussianBlur(gray, (5, 5), 0)
    edges = cv2.Canny(blurred, threshold1=30, threshold2=100)
    
    contours, _ = cv2.findContours(edges, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    
    for contour in contours:
        area = cv2.contourArea(contour)
        if area > min_area_threshold:  # Define a suitable threshold based on your application
            x, y, w, h = cv2.boundingRect(contour)
            aspect_ratio = w / h
            if aspect_ratio <= max_aspect_ratio:  # Define a suitable threshold
                cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)
    
    return frame

# Open camera feed
cap = cv2.VideoCapture(0)

while True:
    ret, frame = cap.read()
    if not ret:
        break
    
    processed_frame = detect_holes(frame)
    
    cv2.imshow('Hole Detection', processed_frame)
    
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

cap.release()
cv2.destroyAllWindows()
