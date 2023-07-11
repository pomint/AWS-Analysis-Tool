import json
import pandas as pd
import sys
import os

def analyze_acl_configuration(config_file, output_folder):
    with open(config_file, 'r') as f:
        config_data = json.load(f)

        # Analyze ACLs
        if 'NetworkAcls' in config_data:
            acls = config_data['NetworkAcls']
            acl_results = []

            for acl in acls:
                acl_id = acl.get('NetworkAclId')
                vpc_id = acl.get('VpcId')
                entries = acl.get('Entries')

                # Analyze ingress ACL entries
                ingress_entries = [entry for entry in entries if entry.get('Egress') is False]
                analyze_acl_entries(acl_id, vpc_id, ingress_entries, 'Ingress', acl_results)

                # Analyze egress ACL entries
                egress_entries = [entry for entry in entries if entry.get('Egress') is True]
                analyze_acl_entries(acl_id, vpc_id, egress_entries, 'Egress', acl_results)

            # Convert analysis results to a DataFrame using Pandas
            df = pd.DataFrame(acl_results, columns=[
                'ACL ID', 'VPC ID', 'Direction', 'Rule Number', 'Protocol', 'Protocol Status',
                'Protocol Details', 'Action', 'Action Status', 'Action Details',
                'CIDR Block', 'CIDR Block Status', 'CIDR Block Details'
            ])

            # Export DataFrame to an Excel file in the specified output folder
            excel_file = os.path.join(output_folder, 'acl_analysis.xlsx')
            df.to_excel(excel_file, index=False)

            print(f"ACL analysis results exported to {excel_file}")

def analyze_acl_entries(acl_id, vpc_id, entries, direction, acl_results):
    for entry in entries:
        rule_number = entry.get('RuleNumber')
        protocol = entry.get('Protocol')
        action = entry.get('RuleAction')
        cidr_block = entry.get('CidrBlock')

        # Perform detailed analysis on ACL entries
        analysis = {}

        # Example: Analyze protocol
        protocol_analysis = analyze_protocol(protocol)
        analysis.update(protocol_analysis)

        # Example: Analyze action
        action_analysis = analyze_action(action)
        analysis.update(action_analysis)

        # Example: Analyze CIDR block
        cidr_analysis = analyze_cidr_block(cidr_block)
        analysis.update(cidr_analysis)

        # Handle empty CIDR Block
        if not cidr_block:
            cidr_block = 'Not Defined'

        # Extract analysis results into separate columns
        acl_results.append({
            'ACL ID': acl_id,
            'VPC ID': vpc_id,
            'Direction': direction,
            'Rule Number': rule_number,
            'CIDR Block': cidr_block,
            'Protocol': protocol,
            'Protocol Status': analysis.get('Protocol Status'),
            'Protocol Details': analysis.get('Protocol Details'),
            'Action': action,
            'Action Status': analysis.get('Action Status'),
            'Action Details': analysis.get('Action Details'),
            'CIDR Block Status': analysis.get('CIDR Block Status'),
            'CIDR Block Details': analysis.get('CIDR Block Details')
        })

def analyze_protocol(protocol):
    # Example analysis logic for protocol
    analysis = {}

    if protocol == '-1':
        analysis['Protocol Status'] = 'Any'
        analysis['Protocol Details'] = 'The protocol allows any type of traffic.'
    else:
        analysis['Protocol Status'] = 'Specific'
        analysis['Protocol Details'] = 'The protocol is limited to a specific type of traffic.'

    return analysis

def analyze_action(action):
    # Example analysis logic for action
    analysis = {}

    if action == 'allow':
        analysis['Action Status'] = 'Allow'
        analysis['Action Details'] = 'The rule allows traffic.'
    else:
        analysis['Action Status'] = 'Deny'
        analysis['Action Details'] = 'The rule denies traffic.'

    return analysis

def analyze_cidr_block(cidr_block):
    # Example analysis logic for CIDR block
    analysis = {}

    if cidr_block == '0.0.0.0/0':
        analysis['CIDR Block Status'] = 'Any'
        analysis['CIDR Block Details'] = 'The rule allows traffic from any source.'
    elif not cidr_block:
        cidr_block = 'Not Defined'
        analysis['CIDR Block Status'] = 'Not Defined'
        analysis['CIDR Block Details'] = 'The CIDR block is not defined.'
    else:
        analysis['CIDR Block Status'] = 'Specific'
        analysis['CIDR Block Details'] = 'The rule restricts traffic to a specific source.'

    return analysis

# Usage: Provide the path to the exported configuration file as a command-line argument
if __name__ == '__main__':
    if len(sys.argv) >= 3:
        config_file = sys.argv[1]
        output_folder = sys.argv[2]
        analyze_acl_configuration(config_file, output_folder)
    else:
        print("Usage: python ACL_analysis.py <config_file> <output_folder>")
