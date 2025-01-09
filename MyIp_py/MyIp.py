import json
def lambda_handler(event, context):
    ip = event['requestContext']['identity']['sourceIp']
    response = {
        'statusCode': 200,
        'body': json.dumps(ip)
    }
    return response
