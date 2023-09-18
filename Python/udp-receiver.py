import socket
import glob
import os.path
import mouse
from PIL import Image
from clip_interrogator import Config, Interrogator


UDP_IP_ADDRESS = "127.0.0.1"
UDP_PORT_NO = 8051

serverSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverSock.bind((UDP_IP_ADDRESS, UDP_PORT_NO))

while True:
    data, addr = serverSock.recvfrom(1024)
    print ("message: ", data)
    
    if (data == b'txt2img'):
        mouse.click('left')
        print('click')      
    
    if (data == b'img2img'):
        folder_path = r'C:\Users\Chynvero\Pictures'
        file_type = r'\*png'
        files = glob.glob(folder_path + file_type)
        max_file = max(files, key=os.path.getctime)

        print(max_file)

        image = Image.open(max_file).convert('RGB')
        ci = Interrogator(Config(clip_model_name="ViT-L-14/openai"))
        prompt = ci.interrogate(image)
        print(prompt)

        #open text file
        text_file = open("C:/Projects/ci/prompt.txt", "w")

        #write string to file
        text_file.write(prompt)
        print('written to file')

        #close file
        text_file.close()
        
        mouse.click('left')
        print('click')
        