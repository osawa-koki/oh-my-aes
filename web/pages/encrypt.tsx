import { useState } from "react";
import { Alert, Button, Form, Spinner } from "react-bootstrap";
import Layout from "../components/Layout";
import setting from "../setting";
import hasWhitespaceAtEitherEnd from "../src/hasWhitespaceAtEitherEnd";
import { MyResponseType } from "../src/ResponseType";

const cipher_modes = ['ECB', 'CBC', 'CFB'];
const cipher_key_sizes = ['128', '192', '256'];

export default function EncryptPage() {

  const [content, setContent] = useState<string>('こんにちは！');
  const [key, setKey] = useState<string>('my-key');
  const [mode, setMode] = useState<string>('ECB');
  const [key_size, setKeySize] = useState<string>('256');
  const [encrypted, setEncrypted] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [copied, setCopied] = useState<boolean>(false);
  const [waiting, setWaiting] = useState<boolean>(false);

  const Encrypt = async () => {
    try {
      setWaiting(true);
      setEncrypted(null);
      setError(null);
      await new Promise(resolve => setTimeout(resolve, setting.smallWaitingTime));
      fetch(`${setting.apiPath}/api/cipher/aes/encrypt/${mode}/${key_size}?key=${key}&data=${content}`)
        .then(res => res.json())
        .then((json: MyResponseType) => {
          setEncrypted(json.encrypted);
        })
        .catch(e => {
          setError(e.message);
        }).finally(() => {
          setWaiting(false);
        });
    } catch (e) {
      setError(e.message);
    }
  };

  return (
    <Layout>
      <div id="Encrypt" className="mt-3">
        <h1>暗号化ツール</h1>
        <Form.Group className="mt-3">
          <Form.Label>暗号化対象文字列</Form.Label>
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
        {
          hasWhitespaceAtEitherEnd(content) && (
            <Alert variant="warning" className="mt-3">
              暗号化対象文字列の両端のいずれか、ないしは両方に空白類似文字が含まれています。
            </Alert>
          )
        }
        {
          waiting ? (
            <div className="mt-3 d-flex justify-content-between">
              <Spinner animation="border" variant="primary" />
              <Spinner animation="border" variant="secondary" />
              <Spinner animation="border" variant="success" />
              <Spinner animation="border" variant="danger" />
              <Spinner animation="border" variant="warning" />
              <Spinner animation="border" variant="info" />
              <Spinner animation="border" variant="light" />
              <Spinner animation="border" variant="dark" />
            </div>
          ) : (
            <Button variant="primary" onClick={Encrypt} className="mt-3 d-block m-auto">暗号化 🔏</Button>
          )
        }
        <hr />
        {
          error !== null ? (
            <>
              <Alert variant="danger" className="mt-3">
                {error}
              </Alert>
            </>
          ) : (
            <>
            {
              encrypted !== null && (
                <>
                  <Alert variant="info" className="mt-3">
                    暗号化された文字列は以下の通りです。
                  </Alert>
                  <Form.Group className="mt-3">
                    <Form.Label>暗号化された文字列</Form.Label>
                    <Form.Control as="textarea" value={encrypted} rows={5} />
                  </Form.Group>
                  <Button variant="warning" className="mt-3 d-block m-auto" onClick={async () => {
                    setCopied(true);
                    navigator.clipboard.writeText(encrypted);
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
