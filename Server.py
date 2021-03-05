#!/usr/bin/env python3
"""Server for multithreaded (asynchronous) chat application."""
from socket import AF_INET, socket, SOCK_STREAM
from threading import Thread
import time
import sys
import json
from collections import namedtuple

class Player:
    Color = None
    Score = 0
    Cards = []
    SelectedCard = None
    BodyPosition = None
    BodyRotation = None
    RHandPosition = None
    RHandRotation = None
    LHandPosition = None
    LHandRotation = None

    def __init__(self, color):
        self.Color = color
        self.Score = 0
        self.Cards = []
        self.SelectedCard = None
        self.BodyPosition = None
        self.BodyRotation = None
        self.RHandPosition = None
        self.RHandRotation = None
        self.LHandPosition = None
        self.LHandRotation = None

    @classmethod
    def fromDict(self, d):
        t = Player(None)
        t.Color = d.Color
        t.Score = d.Score
        t.Cards = d.Cards
        t.SelectedCard = d.SelectedCard
        t.BodyPosition = d.BodyPosition
        t.BodyRotation = d.BodyRotation
        t.RHandPosition = d.RHandPosition
        t.RHandRotation = d.RHandRotation
        t.LHandPosition = d.LHandPosition
        t.LHandRotation = d.LHandRotation
        return t

class Game:
    MaxWaitingTime = 60
    WaitingTime = 60
    CurrentQuestion = 0
    ExpressionId = None
    Host = None
    # Players = []

    def __init__(self):
        self.MaxWaitingTime = 60
        self.WaitingTime = 60
        self.CurrentQuestion = 0
        self.ExpressionId = None
        self.Host = None
        # self.Players = []

    @classmethod
    def fromDict(self, d):
        t = Game()
        t.MaxWaitingTime = d.MaxWaitingTime
        t.WaitingTime = d.WaitingTime
        t.CurrentQuestion = d.CurrentQuestion
        t.ExpressionId = d.ExpressionId
        t.Host = d.Host
        return t

players = []
game = Game()

def accept_incoming_connections():
    """Sets up handling for incoming clients."""
    while True:
        client, client_address = SERVER.accept()
        print("%s:%s has connected." % client_address)
        addresses[client] = client_address
        Thread(target=handle_client, args=(client,)).start()


def handle_client(client):  # Takes client socket as argument.
    """Handles a single client connection."""

    color = 1
    for x in range(1, 5):
        is_existed = False
        for p in players:
            if x == p.Color:
                is_existed = True
                break
        
        if is_existed:
            continue
        color = x
        if game.Host == None:
            game.Host = x
        break


    p = Player(color)
    players.append(p)
    clients[client] = color

    sendToClient(bytes("{Player}" + json.dumps(p.__dict__), "utf8"), client)
    broadcast(bytes("{Game}" + json.dumps(game , default=lambda o: o.__dict__), "utf8"))
    broadcast(bytes("{Players}" + json.dumps(players , default=lambda o: o.__dict__), "utf8"))

    while True:
        # try:
        #     ready_to_read, ready_to_write, in_error = select.select([client,], [client,], [], 5)
        # except select.error:
        #     client.shutdown(2)    # 0 = done receiving, 1 = done sending, 2 = both
        #     client.close()
        #     # connection error event here, maybe reconnect
        #     print(f'{clients[client]} left')
        #     del clients[client]
        #     break

        """ Loop to decide how to forward packet"""
        try:
            msg = client.recv(BUFSIZ)
        except:
        # if(clients != None):
            if client in clients:
                client.close()
                print(f'handle_client: Player {clients[client]} left')

                if(game.Host == clients[client]):
                    pass
                    #end game

                
                player = None
                for p in players:
                    if(p.Color == clients[client]):
                        player = p
                        break
                
                # for key in dels: del players[key]
                if player != None and player in players:
                    players.remove(player)

                del clients[client]
            break

        decodedMsg = msg.decode()
        decodedMsgs = decodedMsg.split("{break}")

        for x in decodedMsgs:
            if(handle_message(x, client) == False):
                break

        # print(f"{clients[client]}: {msg.decode()}")
        # if msg != bytes("{quit}", "utf8"):
            # Handle file transferf8") in msg):
            # if(bytes("{file}","utf8") in msg):
            #     file_name = msg.decode('utf8').split(None, 1)[-1]
            #     notif = "%s has shared file %s" % (name, file_name)
            #     broadcast(bytes(notif, "utf8"))
            # elif(bytes("{content}", "utf8") in msg):
            #     broadcast(msg, skip=client)
            # elif(bytes("{file_done}","utf8") in msg):
            #     broadcast(msg, skip=client)
            # elif(bytes("{fname}", "utf8") in msg):
            #     broadcast(msg, skip=client)
            #     # sleep to prevent this packet mixed with other packets due to speed
            #     time.sleep(0.1)
            # else : 
            #     broadcast(msg, name+": ")
        #     broadcast(msg,str(color)+": ")
        # else:
        #     """ Handle user disconnecting"""
            # print(f'{clients[client]} quited')
            # # client.send(bytes("{quit}", "utf8"))
            # client.close()
            # del clients[client]
            # broadcast(bytes("%s has left the chat." % name, "utf8"))
            # break
    # print(clients)

def handle_message(msg, client):
    if(msg[0:7] == "{CGame}"):
        if(clients[client] == game.Host):
            t = Game.fromDict(json.loads(msg[7:], object_hook=customObjectDecoder))
            game.WaitingTime = t.WaitingTime
            game.CurrentQuestion = t.CurrentQuestion
            game.ExpressionId = t.ExpressionId

            broadcast(bytes("{Game}" + json.dumps(game , default=lambda o: o.__dict__), "utf8"), skip=client)

    elif (msg[0:9] == "{CPlayer}"):

        print(msg[9:])
        t = Player.fromDict(json.loads(msg[9:], object_hook=customObjectDecoder))
        
        idx = None
        for i in range(len(players)):
            if(players[i].Color == t.Color):
                idx = i
                break
        if(idx != None):
            players[i] = t
        
            broadcast(bytes("{Players}" + json.dumps(players , default=lambda o: o.__dict__), "utf8"), skip=client)


def sendToClient(b_msg, client):
    global clients

    try:
        client.send(b_msg + bytes("{break}", "utf8"))
    except:
        
        if(clients != None):
            print(f'sendToClient: Player {clients[client]} left')
            player = None
            for p in players:
                if(p.Color == clients[client]):
                    player = p
                    break
            
            # for key in dels: del players[key]
            if player != None and player in players:
                players.remove(player)

            if(game.Host == clients[client]):
                pass
                #end game

        client.close()
        del clients[client]
        # if(clients != None):
        #     clients = [key for key in clients if key == client]
        #     # for key in delete: del clients[key]


def broadcast(msg, prefix="", skip=None):  # prefix is for name identification.
    """
    Broadcasts a message to clients.
    prefix : string for name identification
    skip : socket instance for skipping forwarding to specific client 
    """
    for sock in list(clients):
        if(sock == None or sock == skip or sock is skip):
            continue
        sendToClient(bytes(prefix, "utf8")+msg, sock)
        #sock.send(bytes(prefix, "utf8")+msg)

def try_connect_port(addr):
    try:
        SERVER.bind(ADDR)
        SERVER.shutdown(2)
        return False
    except:
        return True

def customObjectDecoder(dictObj):
    return namedtuple('X', dictObj.keys())(*dictObj.values())

def customPlayerDecoder(dictObj):
    return namedtuple('Player', dictObj.keys())(*dictObj.values())

def customGameDecoder(dictObj):
    return namedtuple('Game', dictObj.keys())(*dictObj.values())

clients = dict()
addresses = {}

HOST = ''
PORT = 33000
BUFSIZ = int(1e7) + 10
ADDR = (HOST, PORT)

SERVER = socket(AF_INET, SOCK_STREAM)

# a_socket = socket(AF_INET, SOCK_STREAM)

# while try_connect_port(ADDR):
#     pass


# label .retry
# try:
#     SERVER.bind(ADDR)
# except:
#     goto .retry
# while not SERVER.connect_ex(ADDR):
#     pass

SERVER.bind(ADDR)

if __name__ == "__main__":
    """spawn threads for sockets """
    SERVER.listen(5)
    print("Waiting for connection...")
    ACCEPT_THREAD = Thread(target=accept_incoming_connections)
    ACCEPT_THREAD.start()
    ACCEPT_THREAD.join()
SERVER.close()