import { useState } from "react";
import { Alert, Button, Form, Spinner } from "react-bootstrap";
import Layout from "../components/Layout";
import setting from "../setting";
import { MyResponseType } from "../src/ResponseType";

const cipher_modes = ['ECB', 'CBC', 'CFB'];
const cipher_key_sizes = ['128', '192', '256'];

export default function EncryptPage() {

  const [content, setContent] = useState<string>('UHitH3n3gDg5cn1SCsSFanIuZSRXK2hX7G+opdRbivM=');
  const [key, setKey] = useState<string>('my-key');
  const [iv, setIv] = useState<string>('');
  const [mode, setMode] = useState<string>('ECB');
  const [key_size, setKeySize] = useState<string>('256');
  const [decrypted, setDecrypted] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [copied, setCopied] = useState<boolean>(false);
  const [waiting, setWaiting] = useState<boolean>(false);

  const Encrypt = async () => {
    try {
      setWaiting(true);
      setDecrypted(null);
      setError(null);
      await new Promise(resolve => setTimeout(resolve, setting.smallWaitingTime));
      await fetch(`${setting.apiPath}/api/cipher/aes/decrypt/${mode}/${key_size}?key=${key}&data=${encodeURIComponent(content)}&iv=${mode === 'ECB' ? '' : iv}}`)
        .then(async (res) => {
          if (res.status === 500) {
            throw new Error(`サーバサイドで予期せぬエラーが発生しました。`);
          }
          if (res.status === 400) {
            throw new Error(`復号できないデータです。`);
          }
          if (res.status !== 200) {
            throw new Error(`${res.statusText}`);
          }
          return res.json();
        })
        .then((json: MyResponseType) => {
          setError(null);
          setDecrypted(json.decrypted);
        })
        .catch(e => {
          setError(`${e.message}`);
        }).finally(() => {
          setWaiting(false);
        });
    } catch (e) {
      setError(`${e}`);
    }
  };

  return (
    <Layout>
      <div id="Encrypt" className="mt-3">
        <h1>復号ツール</h1>
        <Form.Group className="mt-3">
          <Form.Label>暗号化された文字列</Form.Label>
          <Form.Control as="textarea" rows={5} value={content} onInput={(e) => {setContent((e.target as HTMLTextAreaElement).value)}} />
        </Form.Group>
        <Form.Group className="mt-3">
          <Form.Label>暗号化キー</Form.Label>
          <Form.Control type="text" placeholder="Enter Key" value={key} onInput={(e) => {setKey((e.target as HTMLTextAreaElement).value)}} />
        </Form.Group>
        <hr />
        <Form.Group className="mt-3 d-flex">
          <Form.Label className="w-50">暗号化モード</Form.Label>
          <Form.Select aria-label="Select Cipher Mode" value={mode} onInput={(e) => {setMode((e.target as HTMLSelectElement).value)}}>
            {
              cipher_modes.map((_mode, _) => {
                return <option key={_mode}>{_mode}</option>;
              })
            }
          </Form.Select>
        </Form.Group>
        <Form.Group className="mt-3 d-flex">
          <Form.Label className="w-50">ビットサイズ</Form.Label>
          <Form.Select aria-label="Select Cipher Key Size" onInput={(e) => {setKeySize((e.target as HTMLSelectElement).value)}}>
            {
              cipher_key_sizes.map((_key_size, _) => {
                return <option key={_key_size}>{_key_size}</option>;
              })
            }
          </Form.Select>
        </Form.Group>
        <Form.Group className="mt-3 d-flex">
          <Form.Label className="w-50">IV</Form.Label>
          <Form.Control type="text" placeholder={mode === 'ECB' ? "ECB mode does't use IV." : "Enter IV"} value={iv} onInput={(e) => {setIv((e.target as HTMLTextAreaElement).value)}} disabled={mode === 'ECB'} />
        </Form.Group>
        {
          waiting ? (
            <div className="mt-3 d-flex justify-content-between">
              <Spinner animation="grow" variant="primary" />
              <Spinner animation="grow" variant="secondary" />
              <Spinner animation="grow" variant="success" />
              <Spinner animation="grow" variant="danger" />
              <Spinner animation="grow" variant="warning" />
              <Spinner animation="grow" variant="info" />
              <Spinner animation="grow" variant="light" />
              <Spinner animation="grow" variant="dark" />
            </div>
          ) : (
            <Button variant="primary" onClick={Encrypt} className="mt-3 d-block m-auto">復号 🔏</Button>
          )
        }
        <hr />
        {
          error !== null ? (
            <>
              <Alert variant="danger" className="mt-3">
                <Alert.Heading>復号に失敗しました。</Alert.Heading>
                {error}
              </Alert>
            </>
          ) : (
            <>
            {
              decrypted !== null && (
                <>
                  <Alert variant="info" className="mt-3">
                  復号された文字列は以下の通りです。
                  </Alert>
                  <Form.Group className="mt-3">
                    <Form.Label>復号された文字列</Form.Label>
                    <Form.Control as="textarea" value={decrypted} rows={5} />
                  </Form.Group>
                  <Button variant="warning" className="mt-3 d-block m-auto" onClick={async () => {
                    setCopied(true);
                    navigator.clipboard.writeText(decrypted);
                    await new Promise(resolve => setTimeout(resolve, setting.waitingTime));
                    setCopied(false);
                  }} disabled={copied}>
                    {copied ? 'コピーしました' : 'コピー'}
                  </Button>
                </>
              )
            }
            </>
          )
        }
      </div>
    </Layout>
  );
};
