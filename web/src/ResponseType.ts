
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
  cipherMethod: MyCipherMethod;
  cipherAlgo: MyCipherAlgo;
  cipherMode: MyCipherMode;
  cipherLength: number;
  key: string;
  iv?: string;
  data: string;
  encrypted?: string;
  decrypted?: string;
}

export type { MyResponseType, MyCipherMethod, MyCipherAlgo, MyCipherMode };
