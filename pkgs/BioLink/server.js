import express from 'express';
import fs from 'fs';
import path from 'path';
import crypto from 'crypto';

const app = express();
const PORT = process.env.PORT || 6969;
const WEB_ROOT = process.env.WEB_ROOT || '/var/lib/biolink';

const ENCRYPTION_KEY = crypto
  .createHash('sha256')
  .update('Use-a-random-key-skid') // Remember to gen a key.
  .digest();
const IV_LENGTH = 16;

function encrypt(text) {
  const iv = crypto.randomBytes(IV_LENGTH);
  const cipher = crypto.createCipheriv('aes-256-cbc', ENCRYPTION_KEY, iv);
  let encrypted = cipher.update(text, 'utf8', 'hex');
  encrypted += cipher.final('hex');
  return iv.toString('hex') + ':' + encrypted;
}

function decrypt(enc) {
  const [ivHex, encrypted] = enc.split(':');
  const iv = Buffer.from(ivHex, 'hex');
  const decipher = crypto.createDecipheriv('aes-256-cbc', ENCRYPTION_KEY, iv);
  let decrypted = decipher.update(encrypted, 'hex', 'utf8');
  decrypted += decipher.final('utf8');
  return decrypted;
}

const webroot = path.join(WEB_ROOT, 'public');
app.use(express.static(webroot));

const dbPath = path.join(WEB_ROOT, 'views.json');

if (!fs.existsSync(dbPath)) {
  fs.writeFileSync(
    dbPath,
    JSON.stringify({ views: 0, ips: [] }, null, 2)
  );
}

function readDB() {
  return JSON.parse(fs.readFileSync(dbPath));
}

function writeDB(data) {
  fs.writeFileSync(dbPath, JSON.stringify(data, null, 2));
}

function incrementViews(ip) {
  const data = readDB();

  const encryptedIPs = data.ips || [];
  const alreadySeen = encryptedIPs.some((eip) => decrypt(eip) === ip);

  if (!alreadySeen) {
    data.views++;
    data.ips.push(encrypt(ip));
    writeDB(data);
  }

  return data.views;
}

function getClientIp(req) {
  const cf = req.get('cf-connecting-ip');
  if (cf)
    return cf.trim();

  const real = req.get('x-real-ip');
  if (real)
    return real.trim();

  return req.socket?.remoteAddress || req.connection?.remoteAddress || '';
}

app.get('/api/views', (req, res) => {
  const ip = getClientIp(req);
  const updated = incrementViews(ip);
  res.json({ views: updated });
});

app.listen(PORT, () => console.log(`Server running on port ${PORT}`));