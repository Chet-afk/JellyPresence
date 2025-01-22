def main():
    make_file: str = ""
    while make_file.lower() != 'y' and make_file.lower() != 'n':
        make_file = input("Create / overwrite .env file? (y/n): ")

    if make_file == 'n':
        print("File will not be made.")
        return

    with open('./.env','w') as env:
        client_id: str = input("Enter discord application ID: ")
        jellyAPIkey: str = input("Enter your Jellyfin API key: ")
        jellyURL: str = input("Example: http://your_jellyfin_url:8096\nEnter your Jellyfin URL (include http / https and port): ")

        env.write(f"CLIENTID={client_id}\n")
        env.write(f"JELLYAPIKEY={jellyAPIkey}\n")
        env.write(f"JELLYURL={jellyURL}\n")

    print(".env file created.")
    return



if __name__ == "__main__":
    main()