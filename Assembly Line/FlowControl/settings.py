import csv
import os

# Create or load the CSV file
settings_file_path = "settings.csv"

def write_setting(setting_name: str, value):
    with open(settings_file_path, "a", newline="") as csvfile:
        csv_writer = csv.writer(csvfile)
        csv_writer.writerow([setting_name, value])

def read_settings():
    settings = {}
    with open(settings_file_path, "r") as csvfile:
        csv_reader = csv.reader(csvfile)
        next(csv_reader)  # Skip the header
        for row in csv_reader:
            setting_name, value = row
            settings[setting_name] = value
    return settings

def get_setting_value(setting_name):
    with open(settings_file_path, "r") as csvfile:
        csv_reader = csv.reader(csvfile)
        next(csv_reader)  # Skip the header
        for row in csv_reader:
            if row[0] == setting_name:
                return row[1]  # Return the value if setting_name is found
    return None  # Return None if setting_name is not found

def check_and_create_file():
    if not os.path.exists(settings_file_path):
        # File doesn't exist, so create it and write the header
        with open(settings_file_path, "w", newline="") as csvfile:
            csv_writer = csv.writer(csvfile)
            csv_writer.writerow(["setting_name", "value"])
