import requests, sys;

def Parse(data):
    #will parse through json file, and will create multiple files after every 1,000 entries
    file = open("../Data/0-1000.txt","w");
    print("parsing...", end ="");
    i = 0; 
    j = 0;
    last = i-j;
    while i < len(data):
        try:
            file.write(data[i]['time'] + "," + str(data[i]['borough']) +  "," + str(data[i]['number_of_persons_injured']) + "\n");
            j+=1;
            if(j%1000==0):
                file.close();
                file = open("../Data/"+str(j)+"-"+str(j+1000)+".txt","w");
                
        except KeyError:
            #will show the key errors in the terminal
            print(".", end = "");
        
        i+=1;        
    print("Collected " + str(j) + " amount pieces of data");
    
    
def main():
    #increase the limit by putting $limit=x, and likewise from specific date put 
    response = requests.get('https://data.cityofnewyork.us/resource/qiz3-axqb.json?$limit=50000');
    data = response.json();
    
    Parse(data);
        
if __name__ == "__main__":
    main();