
enum MyCipherMethod {
  Encrypt,
  Decrypt,
}

enum MyCipherAlgo {
  AES
}

enum MyCipherMode {
  ECB
}

interface MyResponseType {
  CipherMethod: MyCipherMethod;
  CipherAlgo: MyCipherAlgo;
  CipherMode: MyCipherMode;
  CipherLength: number;
  Key: string;
  IV?: string;
  Data: string;
  Encrypted?: string;
  Decrypted?: string;
}

export type { MyResponseType, MyCipherMethod, MyCipherAlgo, MyCipherMode };
