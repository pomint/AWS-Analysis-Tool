import json
import pandas as pd
import sys
import os

def analyze_subnet_configuration(config_file, output_folder):
    with open(config_file, 'r') as f:
        config_data = json.load(f)

        if 'Subnets' in config_data:
            subnets = config_data['Subnets']
            analysis_results = []

            for subnet in subnets:
                subnet_id = subnet.get('SubnetId')
                vpc_id = subnet.get('VpcId')
                cidr_block = subnet.get('CidrBlock')
                availability_zone = subnet.get('AvailabilityZone')
                state = subnet.get('State')
                default_for_az = subnet.get('DefaultForAz')
                map_public_ip = subnet.get('MapPublicIpOnLaunch')
                tags = subnet.get('Tags', [])
                name_tag_value = get_tag_value_by_key(tags, 'Name')
                analyze_subnet(subnet_id, vpc_id, cidr_block, availability_zone, state, default_for_az, map_public_ip, name_tag_value, analysis_results)

            # Convert analysis results to a DataFrame using Pandas
            df = pd.DataFrame(analysis_results, columns=[
                'Subnet ID', 'VPC ID', 'CIDR Block', 'Availability Zone', 'State',
                'Default for AZ', 'Map Public IP', 'Name', 'CIDR Block Status', 'CIDR Block Details', 'Recommendation'
            ])

           # Export DataFrame to an Excel file in the specified output folder
            excel_file = os.path.join(output_folder, 'subnet_analysis.xlsx')
            df.to_excel(excel_file, index=False)

            print(f"Subnet analysis results exported to: {excel_file}")

def get_tag_value_by_key(tags, key):
    for tag in tags:
        if tag['Key'] == key:
            return tag['Value']
    return ''

def analyze_subnet(subnet_id, vpc_id, cidr_block, availability_zone, state, default_for_az, map_public_ip, name_tag_value, analysis_results):
    # Perform detailed analysis on the subnet
    analysis = {}

    # Example: Analyze CIDR block
    cidr_analysis = analyze_cidr_block(cidr_block)
    analysis.update(cidr_analysis)

    # Extract analysis results into separate columns
    recommendation = get_recommendation(analysis)
    analysis_results.append({
        'Subnet ID': subnet_id,
        'VPC ID': vpc_id,
        'CIDR Block': cidr_block,
        'Availability Zone': availability_zone,
        'State': state,
        'Default for AZ': default_for_az,
        'Map Public IP': map_public_ip,
        'Name': name_tag_value,
        'CIDR Block Status': analysis.get('CIDR Block Status'),
        'CIDR Block Details': analysis.get('CIDR Block Details'),
        'Recommendation': recommendation
    })

def analyze_cidr_block(cidr_block):
    # Example analysis logic for CIDR block
    analysis = {}

    if cidr_block == '0.0.0.0/0':
        analysis['CIDR Block Status'] = 'Any'
        analysis['CIDR Block Details'] = 'The subnet allows any traffic.'
    elif not cidr_block:
        cidr_block = 'Not Defined'
        analysis['CIDR Block Status'] = 'Not Defined'
        analysis['CIDR Block Details'] = 'The CIDR block is not defined.'
    else:
        analysis['CIDR Block Status'] = 'Specific'
        analysis['CIDR Block Details'] = 'The subnet restricts traffic to a specific range.'

    return analysis

def get_recommendation(analysis):
    if analysis['CIDR Block Status'] == 'Any':
        return 'Consider tightening security by restricting traffic to a specific range.'
    elif analysis['CIDR Block Status'] == 'Not Defined':
        return 'Define a CIDR block for the subnet to properly manage traffic.'
    else:
        return 'No specific recommendation.'

# Usage: Provide the path to the exported configuration file as a command-line argument
if __name__ == '__main__':
    if len(sys.argv) >= 3:
        config_file = sys.argv[1]
        output_folder = sys.argv[2]
        analyze_subnet_configuration(config_file, output_folder)
    else:
        print("Usage: python Subnet_Analysis.py <config_file> <output_folder>")
