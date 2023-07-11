# AWS-Analysis-Tool

1. Extract files to any folder at any location on your device
2. Launch "AWS Analysis.exe"


░█─░█ ░█▀▀▀█ ─█▀▀█ ░█▀▀█ ░█▀▀▀ 
░█─░█ ─▀▀▀▄▄ ░█▄▄█ ░█─▄▄ ░█▀▀▀ 
─▀▄▄▀ ░█▄▄▄█ ░█─░█ ░█▄▄█ ░█▄▄▄

1. Install Python - Version hardcoded in the application: Python 3.11.4
2. Install Python packages to support scripts: Pandas, xlsxwriter
3. Select Folders (Data folder : Folder where your AWS config jsons are stored || Output Folder: Create a folder to store the analysis Results)
4. Analyze Subnets / ACLs / VPCs / Security Groups / Route Tables
5. Exit


░█▀▀█ ░█▀▀▀ ░█▀▀█ ░█─░█ ▀█▀ ░█▀▀█ ░█▀▀▀ ░█▀▀▄   ░█▀▀▀ ▀█▀ ░█─── ░█▀▀▀ ░█▀▀▀█ 
░█▄▄▀ ░█▀▀▀ ░█─░█ ░█─░█ ░█─ ░█▄▄▀ ░█▀▀▀ ░█─░█   ░█▀▀▀ ░█─ ░█─── ░█▀▀▀ ─▀▀▀▄▄ 
░█─░█ ░█▄▄▄ ─▀▀█▄ ─▀▄▄▀ ▄█▄ ░█─░█ ░█▄▄▄ ░█▄▄▀   ░█─── ▄█▄ ░█▄▄█ ░█▄▄▄ ░█▄▄▄█

1. AWS Configuration JSONs (Exported via AWS CLI)
aws --output json ec2 describe_network_acls 
aws --output json ec2 describe_route_tables 
aws --output json ec2 describe_security_groups 
aws --output json ec2 describe_subnets 
aws --output json ec2 describe_vpc_endpoints 
aws --output json ec2 describe_vpcs 
